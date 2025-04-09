using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigameController
{
    /// <summary>
    /// Generer le minijeu
    /// </summary>
    public void GenerateMinigame(int seed);

    /// <summary>
    /// Jouer les �ventuelles animations pour ammener le mini-jeu dans sa configuration de d�part du jeu
    /// </summary>
    public void InitializeMinigame();

    /// <summary>
    /// Active les inputs pour laisser le joueur faire le jeu
    /// </summary>
    public void StartMinigame();


}
