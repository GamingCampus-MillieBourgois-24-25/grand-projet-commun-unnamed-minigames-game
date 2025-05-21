using DG.Tweening;
using UnityEngine;

namespace Axoloop.Global
{
    public class GameManager : SingletonMB<GameManager>
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------



        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(50, 20);
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------



        #endregion
        #region API-----------------------------------------------------------------------------



        #endregion
        #region COROUTINES----------------------------------------------------------------------



        #endregion
    }

}


// Ctrl + M + O pour déplier toutes les régions
#region PROPERTIES----------------------------------------------------------------------



#endregion
#region LIFECYCLE-----------------------------------------------------------------------



#endregion
#region METHODS-------------------------------------------------------------------------



#endregion
#region API-----------------------------------------------------------------------------



#endregion
#region COROUTINES----------------------------------------------------------------------




#endregion
