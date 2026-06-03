using UnityEngine;

public class PlayerPrefsSaveSystem : MonoBehaviour, ISaveSystem
{
    private const string SaveKey = "MergeFlowers_SaveData";
    public static ISaveSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return new SaveData { highScore = 0 };

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<SaveData>(json) ?? new SaveData { highScore = 0 };
    }
}
