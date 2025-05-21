using Axoloop.Global;
using Axoloop.Scripts.Global;
using System;

namespace Assets.Scripts.GLOBAL
{
    public class TransitionMenuManager : BaseSceneManager<TransitionMenuManager>, ISceneManager
    {

        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        public override string SceneName { get => "MAIN_TransitionMinigames"; }

        public override SceneLevel SceneLevel { get => SceneLevel.Level1; }

        public override bool AsyncLoading { get => false; }

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------



        #endregion
        #region METHODS-------------------------------------------------------------------------

        protected override void DisableScene()
        {

        }

        protected override void PlayUnloadTransition()
        {

        }



        #endregion
        #region API-----------------------------------------------------------------------------

        
        #endregion
        #region COROUTINES----------------------------------------------------------------------

        #endregion

    }
}