using Axoloop.Global;
using UnityEngine;

namespace Assets.Code.GLOBAL
{
    public class SceneController : MonoBehaviour
    {
        // Ctrl + M + O pour déplier toutes les régions
        #region PROPERTIES----------------------------------------------------------------------

        [SerializeField] GameObject _LoaderAnimationPrefab;

        private ISceneManager _loadedScene;

        #endregion
        #region LIFECYCLE-----------------------------------------------------------------------

        private void Start()
        {
            
        }

        #endregion
        #region METHODS-------------------------------------------------------------------------



        #endregion
        #region API-----------------------------------------------------------------------------

        public static bool ChangeActiveScene(ISceneManager targetScene)
        {
            return false;
        }

        #endregion
        #region COROUTINES----------------------------------------------------------------------




        #endregion
    }
}