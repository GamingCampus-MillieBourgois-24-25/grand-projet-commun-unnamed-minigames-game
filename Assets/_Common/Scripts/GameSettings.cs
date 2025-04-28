using UnityEngine;
using UnityEngine.EventSystems;

namespace Axoloop.Global
{
    public class GameSettings : SingletonMB<GameSettings>
    {
        //[Header("Developper Settings")]

        //[Tooltip("A utiliser si on a besoin d'une condition pour un éventuel mode Debbug")]
        //[SerializeField] bool _DebugMode = false;

        [Tooltip("A activer pour renseigner une scène de test et la tester facilement avec l'application")]
        [SerializeField] bool _isTesting = false;
        [SerializeField] string testScene;
        //[Tooltip("A activer pour que l'application entière soit exécutée mais que la scène de test soit tout le temps ouverte au lieu du mini-jeu prévu")]
        //[SerializeField] bool _TestInFullApp = false;

        //public static bool DebugMode { get => Instance._DebugMode; }
        public static bool IsTesting { get => Instance._isTesting; }
        public static string TestScene { get => Instance.testScene; }
        //public static bool TestInFullApp { get => Instance._TestInFullApp; }



        // --------------------------------------------------------------------------------------------------------------------------

        [SerializeField] string Start_Screen;
        [SerializeField] string Main_Menu_Scene;
        [SerializeField] string Transition_Scene;
        [SerializeField] string Settings_Scene;
        [SerializeField] string Shop_Scene;
        [SerializeField] string Revive_Scene;

        public static string StartScene { get => Instance.Start_Screen; }
        public static string MainMenuScene { get => Instance.Main_Menu_Scene; }
        public static string TransitionScene { get => Instance.Transition_Scene; }
        public static string SettingsScene { get => Instance.Settings_Scene; }
        public static string ShopScene { get => Instance.Shop_Scene; }
        public static string ReviveScene { get => Instance.Revive_Scene; }


        // --------------------------------------------------------------------------------------------------------------------------

        [Header("UI Settings")]
        [SerializeField] float UI_Object_Fade_Duration = 0.5f;


        public static float UIObjectFadeDuration { get => Instance.UI_Object_Fade_Duration; }

        // --------------------------------------------------------------------------------------------------------------------------

        [Header("Global Objects")]
        [SerializeField] AudioListener _globalAudioListener;
        [SerializeField] EventSystem _eventSystem;

        public static AudioListener GlobalAudioListener { get => Instance._globalAudioListener; }
        public static EventSystem GlobalEventSystem { get => Instance._eventSystem; }

        // --------------------------------------------------------------------------------------------------------------------------


        [SerializeField] int randomInt;
        public static int RandomInt { get => Instance.randomInt; }

        protected override void Awake()
        {
            base.Awake();
            randomInt = Random.Range(0, 1000);
        }

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
