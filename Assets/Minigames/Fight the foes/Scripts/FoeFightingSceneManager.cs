using System;
using Axoloop.Global;
using Axoloop.Scripts.Global;

namespace AxoLoop.Minigames.FightTheFoes
{
    public class FoeFightingSceneManager : BaseSceneManager<FoeFightingSceneManager>, ISceneManager
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        public override string SceneName { get => "MAIN Fight The Foes"; }
             
        public override SceneLevel SceneLevel { get => SceneLevel.Level1; }

        public Action<string> SceneLoaded { get; set; }

        public Action<string> SceneUnloaded { get; set; }



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
        #region COROUTINES----------------------------------------------------------------------

        #endregion

    }
}
