
using AxoLoop.Minigames.FightTheFoes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GameTrigger : MonoBehaviour
{
    #region PROPERTIES ----------------------------------------------------------------------------------------------------------------

    Action TriggerEntered;
    Action TriggerExited;

    #endregion
    #region LIFECYCLE -----------------------------------------------------------------------------------------------------------------

    protected virtual void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        TriggerExited?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        TriggerEntered = null;
        TriggerExited = null;
    }

    #endregion
    #region PUBLIC METHODS ------------------------------------------------------------------------------------------------------------

    public void EnterSubscribe(Action action)
    {
        TriggerEntered += action;
    }
    public void EnterUnsubscribe(Action action)
    {
        TriggerEntered -= action;
    }
    public void ExitSubscribe(Action action)
    {
        TriggerExited += action;
    }
    public void ExitUnsubscribe(Action action)
    {
        TriggerExited -= action;
    }

    #endregion
}

