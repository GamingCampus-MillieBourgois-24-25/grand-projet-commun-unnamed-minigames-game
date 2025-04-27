using AxoLoop.Minigames.MatchTheStars;
using UnityEngine;

public static class MTSUtils
{
    public static MTSDifficulty SetDifficulty(MinigameDifficultyLevel difficulty)
    {
        MTSDifficulty difficulty1;
        switch (difficulty)
        {
            case MinigameDifficultyLevel.FirstTime:
            case MinigameDifficultyLevel.VeryEasy:
                difficulty1 = MTSDifficulty.Easy; break;
            case MinigameDifficultyLevel.Easy:
            case MinigameDifficultyLevel.Medium:
                difficulty1 = MTSDifficulty.Medium; break;
            case MinigameDifficultyLevel.Hard:
            case MinigameDifficultyLevel.VeryHard:
            case MinigameDifficultyLevel.Impossible:
            default: difficulty1 = MTSDifficulty.Hard; break;
        }
        return difficulty1;
    }
}
