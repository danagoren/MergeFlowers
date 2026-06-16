using UnityEngine;

public class FloatingPetal : MonoBehaviour
{
    public float speed = 1f;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (Camera.main.WorldToViewportPoint(transform.position).x > 1.2f)
            FloatingPetalPool.Instance.Release(gameObject);
    }

    private void OnMouseDown()
    {
        Spawner.Instance.SpawnPinkFlower();
        FloatingPetalPool.Instance.Release(gameObject);
    }
}
