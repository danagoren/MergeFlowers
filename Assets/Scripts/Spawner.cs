using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject[] flowerPrefabs;
    public ScoreManager ScoreManager { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPinkFlower(Vector2 worldPos)
    {
        var slot = GridManager.Instance.FindClosestEmptySlot(worldPos);
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
