using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Currency { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }

    public void SpendCurrency(int amount)
    {
        if (Currency >= amount)
            Currency -= amount;
    }
}