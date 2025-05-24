using Axoloop.Global;
using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseMinigameController<T> : SingletonMB<T>, IMinigameController where T : BaseMinigameController<T>
{
    [SerializeField] protected TutorialText tutorialText;

    /// <summary>
    /// Generer le minijeu
    /// </summary>
    protected abstract Task GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel);

    /// <summary>
    /// Jouer les éventuelles animations pour ammener le mini-jeu dans sa configuration de départ du jeu
    /// </summary>
    protected abstract void InitializeMinigame();

    /// <summary>
    /// Active les inputs pour laisser le joueur faire le jeu
    /// </summary>
    protected abstract void StartMinigame();

    public Action OnTutorialSignal { get; set; }
    public abstract Action OnStartSignal { get; set; }
}


