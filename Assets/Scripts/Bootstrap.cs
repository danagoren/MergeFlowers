using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Save System")]
    [SerializeField] private bool _useJsonSave = true;

    public ScoreManager ScoreManager { get; private set; }
    public ISaveSystem SaveSystem { get; private set; }

    private void Awake()
    {
        SaveSystem = _useJsonSave
            ? gameObject.AddComponent<JsonSaveSystem>()
            : gameObject.AddComponent<PlayerPrefsSaveSystem>();

        ScoreManager = new ScoreManager(SaveSystem);
    }
}
