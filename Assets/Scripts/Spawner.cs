using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject[] flowerPrefabs;
    public Sprite buttonIdleSprite;
    public Sprite buttonClickedSprite;

    private Image _buttonImage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var btnGO = GameObject.Find("SpawnButton");
        if (btnGO != null)
        {
            var btn = btnGO.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(SpawnPinkFlower);
                btn.onClick.AddListener(OnButtonClick);
            }

            _buttonImage = btnGO.GetComponent<Image>();
        }
    }

    private void OnButtonClick()
    {
        if (_buttonImage == null || buttonClickedSprite == null) return;
        StartCoroutine(SwapSpriteTemporary());
    }

    private System.Collections.IEnumerator SwapSpriteTemporary()
    {
        var original = _buttonImage.sprite;
        _buttonImage.sprite = buttonClickedSprite;
        yield return new WaitForSeconds(1f);
        if (buttonIdleSprite != null)
            _buttonImage.sprite = buttonIdleSprite;
        else
            _buttonImage.sprite = original;
    }

    public void SpawnPinkFlower()
    {
        var slot = GridManager.Instance.FindRandomEmptySlot();
        if (slot == null)
        {
            Debug.Log("All slots are full!");
            return;
        }

        var (x, y) = slot.Value;
        Vector2 pos = GridManager.Instance.GetCellPosition(x, y);
        pos.y += flowerPrefabs[0].GetComponent<Flower>().slotVerticalOffset;

        GameObject flower = Instantiate(flowerPrefabs[0], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        flower.name = $"Flower_{x}_{y}";

        GridManager.Instance.PlaceItem(flower, x, y);
    }
}
