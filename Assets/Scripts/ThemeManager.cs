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
    [SerializeField] private Spawner _spawner;

    private Theme _currentTheme = Theme.Default;
    private Sprite _defaultBackground;
    private Sprite _defaultFloatingPetal;
    private Sprite[] _defaultSakuraSprites;

    public Sprite CurrentFloatingPetalSprite { get; private set; }
    public Sprite[] CurrentSakuraSprites { get; private set; }

    private void Start()
    {
        _defaultBackground = _background.sprite;
        _defaultFloatingPetal = _floatingPetal.GetComponent<SpriteRenderer>().sprite;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;

        _defaultSakuraSprites = new Sprite[_spawner.sakuraPrefabs.Length];
        for (int i = 0; i < _spawner.sakuraPrefabs.Length; i++)
            _defaultSakuraSprites[i] = _spawner.sakuraPrefabs[i].GetComponent<SpriteRenderer>().sprite;
        CurrentSakuraSprites = _defaultSakuraSprites;
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
        var christmasSprites = new Sprite[8];
        var loadedCount = 0;
        for (int i = 0; i < 8; i++)
        {
            var index = i;
            var handle = Addressables.LoadAssetAsync<Sprite>($"Assets/Sprites/Christmas/SakuraChristmas_{index}.png");
            handle.Completed += op =>
            {
                if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && op.Result != null)
                    christmasSprites[index] = op.Result;
                else
                    Debug.LogError($"ThemeManager: failed to load Christmas sprite {index}");
                loadedCount++;
                if (loadedCount == 8)
                {
                    CurrentSakuraSprites = christmasSprites;
                    UpdateAllSakura();
                }
            };
        }
        _currentTheme = Theme.Christmas;
    }

    private void SetDefaultTheme()
    {
        _background.sprite = _defaultBackground;
        CurrentFloatingPetalSprite = _defaultFloatingPetal;
        _floatingPetal.GetComponent<SpriteRenderer>().sprite = _defaultFloatingPetal;
        UpdateActivePetals();
        CurrentSakuraSprites = _defaultSakuraSprites;
        UpdateAllSakura();
        _currentTheme = Theme.Default;
    }

    private void UpdateActivePetals()
    {
        foreach (var petal in FindObjectsByType<FloatingPetal>(FindObjectsSortMode.None))
            petal.GetComponent<SpriteRenderer>().sprite = CurrentFloatingPetalSprite;
    }

    private void UpdateAllSakura()
    {
        foreach (var s in FindObjectsByType<Sakura>(FindObjectsSortMode.None))
        {
            if (s.tier >= 0 && s.tier < CurrentSakuraSprites.Length)
                s.GetComponent<SpriteRenderer>().sprite = CurrentSakuraSprites[s.tier];
        }
    }
}
