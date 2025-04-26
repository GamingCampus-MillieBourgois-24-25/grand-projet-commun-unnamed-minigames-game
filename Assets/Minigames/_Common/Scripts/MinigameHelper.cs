using UnityEngine;

public static class MinigameHelper
{

    static string timesPlayedKey = "TimesPlayed";

    public static MinigameDifficultyLevel GetDifficulty(MinigameObject minigame)
    {
        int numberOfTimesPlayed = PlayerPrefs.GetInt("" + minigame.name + timesPlayedKey, 0);

        if(numberOfTimesPlayed == 0)
        {
            return MinigameDifficultyLevel.FirstTime;
        }

        int currentScore = ScoreManager.Instance.GetCurrentScore();

        if(currentScore < 3)
            return MinigameDifficultyLevel.VeryEasy;
        if(currentScore < 7)
            return MinigameDifficultyLevel.Easy;
        if (currentScore < 14)
            return MinigameDifficultyLevel.Medium;
        if (currentScore < 21)
            return MinigameDifficultyLevel.Hard;
        if (currentScore < 35)
            return MinigameDifficultyLevel.VeryHard;
        
        return MinigameDifficultyLevel.Impossible;
    }

    public static void IncrementMinigamePlayed(MinigameObject minigame)
    {
        int numberOfTimesPlayed = PlayerPrefs.GetInt("" + minigame.name + timesPlayedKey, 0);
        numberOfTimesPlayed++;
        PlayerPrefs.SetInt("" + minigame.name + "TimesPlayed", numberOfTimesPlayed);
        PlayerPrefs.Save();
    }
}
