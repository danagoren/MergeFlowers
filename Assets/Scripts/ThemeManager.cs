using UnityEngine;
using UnityEngine.AddressableAssets;

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

    private Theme _currentTheme = Theme.Default;
    private Sprite _defaultBackground;
    private Sprite _defaultFloatingPetal;

    public Sprite CurrentFloatingPetalSprite { get; private set; }

    private void Start()
    {
        _defaultBackground = _background.sprite;
        _defaultFloatingPetal = _floatingPetal.GetComponent<SpriteRenderer>().sprite;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;
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
        var bgHandle = Addressables.LoadAssetAsync<Sprite>("Assets/Sprites/BackgroundCristmas.jpg");
        var petalHandle = Addressables.LoadAssetAsync<Sprite>("Assets/Sprites/FloatingPetalChristmas.PNG");
        bgHandle.Completed += op =>
        {
            if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                _background.sprite = op.Result;
        };
        petalHandle.Completed += op =>
        {
            if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                CurrentFloatingPetalSprite = op.Result;
                _floatingPetal.GetComponent<SpriteRenderer>().sprite = op.Result;
                UpdateActivePetals();
            }
        };
        _currentTheme = Theme.Christmas;
    }

    private void SetDefaultTheme()
    {
        _background.sprite = _defaultBackground;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;
        _floatingPetal.GetComponent<SpriteRenderer>().sprite = _defaultFloatingPetal;
        UpdateActivePetals();
        _currentTheme = Theme.Default;
    }

    private void UpdateActivePetals()
    {
        foreach (var petal in FindObjectsByType<FloatingPetal>(FindObjectsSortMode.None))
            petal.GetComponent<SpriteRenderer>().sprite = CurrentFloatingPetalSprite;
    }
}
