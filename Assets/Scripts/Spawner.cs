using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Color circleColor = new Color(1f, 0.41f, 0.71f);
    public float circleScale = 12f;

    private Sprite circleSprite;

    private void Start()
    {
        circleSprite = GenerateCircleSprite();
        CreateUIButton();
    }

    private Sprite GenerateCircleSprite()
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size);
        float radius = size / 2f - 4;
        Vector2 center = new Vector2(size / 2f, size / 2f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                texture.SetPixel(x, y, dist <= radius ? Color.white : Color.clear);
            }
        }
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    private void CreateUIButton()
    {
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }

        GameObject buttonGO = new GameObject("SpawnButton", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonGO.transform.SetParent(canvasGO.transform, false);

        RectTransform rect = buttonGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 50);
        rect.sizeDelta = new Vector2(160, 50);

        Image image = buttonGO.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        GameObject textGO = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(buttonGO.transform, false);
        Text text = textGO.GetComponent<Text>();
        text.text = "Spawn";
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        buttonGO.GetComponent<Button>().onClick.AddListener(SpawnPinkCircle);
    }

    public void SpawnPinkCircle()
    {
        var slot = GridManager.Instance.FindRandomEmptySlot();
        if (slot == null)
        {
            Debug.Log("All slots are full!");
            return;
        }

        var (x, y) = slot.Value;
        Vector2 pos = GridManager.Instance.GetCellPosition(x, y);

        GameObject circle = new GameObject($"Circle_{x}_{y}", typeof(SpriteRenderer));
        circle.transform.position = new Vector3(pos.x, pos.y, 0);
        circle.transform.localScale = new Vector3(circleScale, circleScale, 1);

        SpriteRenderer sr = circle.GetComponent<SpriteRenderer>();
        sr.sprite = circleSprite;
        sr.color = circleColor;
        sr.sortingOrder = 1;

        GridManager.Instance.PlaceItem(circle, x, y);
    }
}
