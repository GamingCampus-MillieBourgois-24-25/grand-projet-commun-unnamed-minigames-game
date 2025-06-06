using System;
using Axoloop.Global;
using Axoloop.Scripts.Global;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class BreakThePlankSceneManager : BaseSceneManager<BreakThePlankSceneManager>, ISceneManager
    {
        // Ctrl + M + O pour d�plier toutes les r�gions
        #region PROPERTIES----------------------------------------------------------------------

        public override string SceneName { get => "AxoTiming"; }
             
        public override SceneLevel SceneLevel { get => SceneLevel.Level1; }

                public override bool AsyncLoading { get => true; }
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

        protected override void PlayLoadTransition()
        {

        }


        #endregion
        #region COROUTINES----------------------------------------------------------------------

        #endregion

    }
}
