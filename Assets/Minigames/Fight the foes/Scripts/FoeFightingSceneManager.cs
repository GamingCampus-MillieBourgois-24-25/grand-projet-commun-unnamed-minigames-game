using System;
using Axoloop.Global;
using Axoloop.Scripts.Global;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingSceneManager : BaseSceneManager<FoeFightingSceneManager>, ISceneManager
    {
        // Ctrl + M + O pour d�plier toutes les r�gions
        #region PROPERTIES----------------------------------------------------------------------

        public override string SceneName { get => "MAIN Fight The Foes"; }
             
        public override SceneLevel SceneLevel { get => SceneLevel.Level1; }
        

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
