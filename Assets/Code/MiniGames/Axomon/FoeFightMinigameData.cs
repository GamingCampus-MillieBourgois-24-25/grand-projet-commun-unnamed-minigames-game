using Axoloop.Global;
using UnityEngine;

public class FoeFightMinigameData : SingletonMB<FoeFightMinigameData>
{
    [SerializeField] Foe currentFoe;
    
    public static Foe CurrentFoe { get => Instance.currentFoe; set => Instance.currentFoe = value; }
}
