using UnityEngine;

public class SaveSystem : MonoBehaviour, ISaveSystem
{
    public static ISaveSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Save(SaveData data)
    {
        // TODO: implement save (e.g., JSON to file)
        Debug.Log("Save called");
    }

    public SaveData Load()
    {
        // TODO: implement load (e.g., JSON from file)
        Debug.Log("Load called");
        return new SaveData();
    }
}
