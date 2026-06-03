using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [Header("Save System")]
    [SerializeField] private bool _useJsonSave = true;

    public ScoreManager ScoreManager { get; private set; }
    public ISaveSystem SaveSystem { get; private set; }

    [SerializeField] private Text _counterText;

    private void Awake()
    {
        SaveSystem = _useJsonSave
            ? gameObject.AddComponent<JsonSaveSystem>()
            : gameObject.AddComponent<PlayerPrefsSaveSystem>();

        ScoreManager = new ScoreManager(SaveSystem);

        var spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
            spawner.ScoreManager = ScoreManager;
    }

    private void Start()
    {
        if (_counterText == null)
        {
            _counterText = GameObject.Find("CurrentScore")?.GetComponent<Text>();
            if (_counterText == null)
                Debug.LogError("Bootstrap: Could not find 'CurrentScore' Text");
        }
    }

    private void Update()
    {
        if (_counterText != null)
            _counterText.text = ScoreManager.CurrentScore.ToString();
    }
}
