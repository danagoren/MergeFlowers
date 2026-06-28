using UnityEngine;

public class FloatingPetal : MonoBehaviour
{
    public float speed = 1f;

    private void OnEnable()
    {
        if (ThemeManager.Instance != null && ThemeManager.Instance.CurrentFloatingPetalSprite != null)
            GetComponent<SpriteRenderer>().sprite = ThemeManager.Instance.CurrentFloatingPetalSprite;
    }

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (Camera.main.WorldToViewportPoint(transform.position).x > 1.2f)
            FloatingPetalPool.Instance.Release(gameObject);
    }

    private void OnMouseDown()
    {
        Spawner.Instance.SpawnPinkSakura(transform.position);
        FloatingPetalPool.Instance.Release(gameObject);
    }
}
