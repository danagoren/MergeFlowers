using UnityEngine;

public class Sakura : MonoBehaviour
{
    public int tier;
    public float slotVerticalOffset;
    public float slotHorizontalOffset;
    public Sprite[] spawnAnimationSprites;
    public float animFrameDuration = 0.1f;

    private Vector3 offset;
    private Vector3 originalPos;
    private int slotX;
    private int slotY;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (tier == 0 && spawnAnimationSprites.Length > 0)
            StartCoroutine(PlaySpawnAnimation());
    }

    private void Update()
    {
        if (CompareTag("Sakura"))
            _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    private System.Collections.IEnumerator PlaySpawnAnimation()
    {
        var original = _spriteRenderer.sprite;
        var originalScale = transform.localScale;
        var originalPos = transform.position;
        transform.localScale = new Vector3(0.22f, 0.22f, 1f);
        transform.position += new Vector3(0, 0.08f, 0);
        foreach (var s in spawnAnimationSprites)
        {
            _spriteRenderer.sprite = s;
            yield return new WaitForSeconds(animFrameDuration);
        }
        _spriteRenderer.sprite = original;
        transform.localScale = originalScale;
        transform.position = originalPos;
    }

    private void OnMouseDown()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, 0);
        originalPos = transform.position;

        var cell = GridManager.Instance.GetCellFromPosition(transform.position);
        if (GridManager.Instance.IsValidCell(cell.x, cell.y))
        {
            slotX = cell.x;
            slotY = cell.y;
            GridManager.Instance.RemoveItem(slotX, slotY);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseWorld.x, mouseWorld.y, 0) + offset;
    }

    private void OnMouseUp()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = new Vector2(mouseWorld.x, mouseWorld.y);

        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);
        GetComponent<Collider2D>().enabled = true;
        if (hit.collider != null)
        {
            Sakura other = hit.collider.GetComponent<Sakura>();
            if (other != null && other != this && other.tier == tier && tier + 1 < Spawner.Instance.sakuraPrefabs.Length)
            {
                MergeSakura(other);
                return;
            }
        }

        var cell = GridManager.Instance.GetCellFromPosition(transform.position);
        if (GridManager.Instance.IsCellEmpty(cell.x, cell.y))
        {
            Vector2 pos = GridManager.Instance.GetCellPosition(cell.x, cell.y);
            pos.y += slotVerticalOffset;
            pos.x += slotHorizontalOffset;
            transform.position = new Vector3(pos.x, pos.y, 0);
            slotX = cell.x;
            slotY = cell.y;
            GridManager.Instance.PlaceItem(gameObject, slotX, slotY);
            return;
        }

        SnapBack();
    }

    private void MergeSakura(Sakura other)
    {
        var targetCell = GridManager.Instance.GetCellFromPosition(other.transform.position);
        GridManager.Instance.RemoveItem(targetCell.x, targetCell.y);

        Destroy(other.gameObject);
        Destroy(gameObject);

        GameObject nextPrefab = Spawner.Instance.sakuraPrefabs[tier + 1];
        Vector3 pos = GridManager.Instance.GetCellPosition(targetCell.x, targetCell.y);
        pos.y += nextPrefab.GetComponent<Sakura>().slotVerticalOffset;
        pos.x += nextPrefab.GetComponent<Sakura>().slotHorizontalOffset;
        GameObject newSakura = Instantiate(nextPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSakura.name = $"Sakura_{targetCell.x}_{targetCell.y}";

        GridManager.Instance.PlaceItem(newSakura, targetCell.x, targetCell.y);
        Spawner.Instance.ScoreManager?.AddScore(1);
    }

    private void SnapBack()
    {
        transform.position = originalPos;
        GridManager.Instance.PlaceItem(gameObject, slotX, slotY);
    }
}
