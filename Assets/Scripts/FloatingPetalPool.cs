using UnityEngine;
using UnityEngine.Pool;

public class FloatingPetalPool : MonoBehaviour
{
    public static FloatingPetalPool Instance { get; private set; }

    public GameObject prefab;
    public int defaultCapacity = 15;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        Instance = this;
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: defaultCapacity
        );

        var temp = new GameObject[defaultCapacity];
        for (int i = 0; i < defaultCapacity; i++)
            temp[i] = _pool.Get();
        for (int i = 0; i < defaultCapacity; i++)
            _pool.Release(temp[i]);
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnAtInterval), 0f, 6f);
    }

    private void SpawnAtInterval()
    {
        Vector3[] positions = {
            new Vector3(-10.2f, 3.3f, 0f),
            new Vector3(-11.1f, 3.2f, 0f),
            new Vector3(-10.45f, 2.7f, 0f),
            new Vector3(-11.3f, 2.25f, 0f),
            new Vector3(-11.9f, 2.5f, 0f),
        };
        foreach (var pos in positions)
            Get(pos);
    }

    public GameObject Get(Vector3 position)
    {
        var obj = _pool.Get();
        obj.transform.position = position;
        return obj;
    }

    public void Release(GameObject obj)
    {
        _pool.Release(obj);
    }
}
