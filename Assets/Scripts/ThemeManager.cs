using UnityEngine;

public enum Theme { Default, Christmas }

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private GameObject _floatingPetal;
    [SerializeField] private GameObject _petalParticles;
    [SerializeField] private GameObject _snowParticles;
    [SerializeField] private Sprite _christmasBackground;
    [SerializeField] private Sprite _christmasFloatingPetal;

    private Theme _currentTheme = Theme.Default;
    private Sprite _defaultBackground;
    private Sprite _defaultFloatingPetal;

    public Sprite CurrentFloatingPetalSprite { get; private set; }

    private void Start()
    {
        _defaultBackground = _background.sprite;
        _defaultFloatingPetal = _floatingPetal.GetComponent<SpriteRenderer>().sprite;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;
        _petalParticles.SetActive(true);
        _snowParticles.SetActive(false);
    }

    public void ToggleTheme()
    {
        switch (_currentTheme)
        {
            case Theme.Default:
                SetChristmasTheme();
                break;
            case Theme.Christmas:
                SetDefaultTheme();
                break;
        }
    }

    private void SetChristmasTheme()
    {
        _background.sprite = _christmasBackground;
        CurrentFloatingPetalSprite = _christmasFloatingPetal;
        _floatingPetal.GetComponent<SpriteRenderer>().sprite = _christmasFloatingPetal;
        UpdateActivePetals();
        _petalParticles.SetActive(false);
        _snowParticles.SetActive(true);
        _currentTheme = Theme.Christmas;
    }

    private void SetDefaultTheme()
    {
        _background.sprite = _defaultBackground;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;
        _floatingPetal.GetComponent<SpriteRenderer>().sprite = _defaultFloatingPetal;
        UpdateActivePetals();
        _petalParticles.SetActive(true);
        _snowParticles.SetActive(false);
        _currentTheme = Theme.Default;
    }

    private void UpdateActivePetals()
    {
        foreach (var petal in FindObjectsByType<FloatingPetal>(FindObjectsSortMode.None))
            petal.GetComponent<SpriteRenderer>().sprite = CurrentFloatingPetalSprite;
    }
}
