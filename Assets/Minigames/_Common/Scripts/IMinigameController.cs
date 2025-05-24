using Axoloop.Global;
using System;
using System.Threading.Tasks;

public interface IMinigameController
{
    public Action OnTutorialSignal { get; set; }
    public Action OnStartSignal { get; set; }
}

public enum MinigameDifficultyLevel
{
    FirstTime,
    VeryEasy,
    Easy,
    Medium,
    Hard,
    VeryHard,
    Impossible
}
