using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject[] sakuraPrefabs;
    public ScoreManager ScoreManager { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPinkSakura(Vector2 worldPos)
    {
        var slot = GridManager.Instance.FindClosestEmptySlot(worldPos);
        if (slot == null)
        {
            Debug.Log("All slots are full!");
            return;
        }

        var (x, y) = slot.Value;
        Vector2 pos = GridManager.Instance.GetCellPosition(x, y);
        pos.y += sakuraPrefabs[0].GetComponent<Sakura>().slotVerticalOffset;

        GameObject sakura = Instantiate(sakuraPrefabs[0], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        sakura.name = $"Sakura_{x}_{y}";

        GridManager.Instance.PlaceItem(sakura, x, y);
    }
}
