using UnityEngine;

public class FloatingPetal : MonoBehaviour
{
    public float speed = 1f;
    public float swaySpeed = 2f;
    public float swayAmount = 0.3f;

    private float swayOffset;

    private void Start()
    {
        swayOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void OnEnable()
    {
        if (ThemeManager.Instance != null && ThemeManager.Instance.CurrentFloatingPetalSprite != null)
            GetComponent<SpriteRenderer>().sprite = ThemeManager.Instance.CurrentFloatingPetalSprite;
    }

    private void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed + swayOffset) * swayAmount;
        transform.position += (Vector3.right * speed + Vector3.up * sway) * Time.deltaTime;

        Vector2 velocity = new Vector2(speed, sway);
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Camera.main.WorldToViewportPoint(transform.position).x > 1.2f)
            FloatingPetalPool.Instance.Release(gameObject);
    }

    private void OnMouseDown()
    {
        Spawner.Instance.SpawnPinkSakura(transform.position);
        FloatingPetalPool.Instance.Release(gameObject);
    }
}
