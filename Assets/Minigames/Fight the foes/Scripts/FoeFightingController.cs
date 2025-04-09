using System.Collections;
using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;

public class FoeFightingController : SingletonMB<FoeFightingController>, IMinigameController
{
    public void GenerateMinigame(int seed)
    {
        Random.InitState(seed);


    }

    public void InitializeMinigame()
    {
    }

    public void StartMinigame()
    {
    }


}
