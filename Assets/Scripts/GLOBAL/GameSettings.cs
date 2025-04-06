using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Axoloop.Global
{
    public class GameSettings : SingletonMB<GameSettings>
    {
        [Header("Developper Settings")]

        [Tooltip("A utiliser si on a besoin d'une condition pour un éventuel mode Debbug")]
        [SerializeField] bool _DebugMode = false;

        [Tooltip("A activer pour renseigner une scène de test et la tester facilement avec l'application")]
        [SerializeField] bool _isTesting = false;
        [SerializeField] SceneAsset _TestingScene;

        [Tooltip("A activer pour que l'application entière soit exécutée mais que la scène de test soit tout le temps ouverte au lieu du mini-jeu prévu")]
        [SerializeField] bool _TestInFullApp = false;

        public static bool DebugMode { get => Instance._DebugMode; }
        public static bool IsTesting { get => Instance._isTesting; }
        public static SceneAsset TestingScene { get => Instance._TestingScene; }
        public static bool TestInFullApp { get => Instance._TestInFullApp; }



        // --------------------------------------------------------------------------------------------------------------------------

        [Header("Scenes Registration")]
        [SerializeField] SceneAsset Start_Screen;
        [SerializeField] SceneAsset Main_Menu_Scene;
        [SerializeField] SceneAsset Transition_Scene;
        [SerializeField] SceneAsset Settings_Scene;
        [SerializeField] SceneAsset Shop_Scene;

        public static SceneAsset StartScene { get => Instance.Start_Screen; }
        public static SceneAsset MainMenuScene { get => Instance.Main_Menu_Scene; }
        public static SceneAsset TransitionScene { get => Instance.Transition_Scene; }
        public static SceneAsset SettingsScene { get => Instance.Settings_Scene; }
        public static SceneAsset ShopScene { get => Instance.Shop_Scene; }


        // --------------------------------------------------------------------------------------------------------------------------

        [Header("UI Settings")]
        [SerializeField] float UI_Object_Fade_Duration = 0.5f;


        public static float UIObjectFadeDuration { get => Instance.UI_Object_Fade_Duration; }

        // --------------------------------------------------------------------------------------------------------------------------
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
