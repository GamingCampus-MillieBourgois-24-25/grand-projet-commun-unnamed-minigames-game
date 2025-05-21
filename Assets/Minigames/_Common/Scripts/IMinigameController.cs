using System.Threading.Tasks;

public interface IMinigameController
{
    /// <summary>
    /// Generer le minijeu
    /// </summary>
    public Task GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel);

    /// <summary>
    /// Jouer les éventuelles animations pour ammener le mini-jeu dans sa configuration de départ du jeu
    /// </summary>
    public void InitializeMinigame();

    /// <summary>
    /// Active les inputs pour laisser le joueur faire le jeu
    /// </summary>
    public void StartMinigame();


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
