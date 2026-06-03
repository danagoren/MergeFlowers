using System;

public class ScoreManager
{
    private readonly ISaveSystem _saveSystem;

    public event Action<int> OnCurrentScoreChanged;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    /*public void start()
    {
        ResetScore();
    }*/

    public ScoreManager(ISaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
        var data = _saveSystem.Load();
        HighScore = data.highScore;
        CurrentScore = 0;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        OnCurrentScoreChanged?.Invoke(CurrentScore);
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            _saveSystem.Save(new SaveData { highScore = HighScore });
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
