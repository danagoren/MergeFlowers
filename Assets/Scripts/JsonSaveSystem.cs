using UnityEngine;
using System.IO;

public class JsonSaveSystem : MonoBehaviour, ISaveSystem
{
    private string FilePath => Path.Combine(Application.persistentDataPath, "save.json");
    public static ISaveSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, json);
    }

    public SaveData Load()
    {
        if (!File.Exists(FilePath))
            return new SaveData { highScore = 0 };

        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<SaveData>(json) ?? new SaveData { highScore = 0 };
    }
}
