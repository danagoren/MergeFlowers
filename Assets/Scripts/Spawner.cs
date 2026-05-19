using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject[] flowerPrefabs;

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
                btn.onClick.AddListener(SpawnPinkFlower);
        }
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

        GameObject flower = Instantiate(flowerPrefabs[0], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        flower.name = $"Flower_{x}_{y}";

        GridManager.Instance.PlaceItem(flower, x, y);
    }
}
