using System;

public class ScoreManager
{
    private readonly ISaveSystem _saveSystem;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

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
