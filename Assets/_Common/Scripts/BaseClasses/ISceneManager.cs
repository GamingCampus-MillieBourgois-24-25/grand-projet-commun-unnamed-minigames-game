using System;

namespace Axoloop.Global
{
    public interface ISceneManager
    {
        /// <summary>
        /// The exact name of the scene to load
        /// </summary>
        public string SceneName { get; }

        /// <summary>
        /// The level of the scene to load.<br></br>
        /// This is used to determine how the scene is loaded.<br></br>
        /// </summary>
        public SceneLevel SceneLevel { get; }


        /// <summary>
        /// This method should handle the loading of the scene and its initialization
        /// </summary>
        public void LoadScene(Action<string> callBack);

        /// <summary>
        /// This method should handle the destruction of the scene, the transition and the unloading of the scene
        /// </summary>
        public void UnloadScene();

        /// <summary>
        /// Fire this event when the scene is loaded and ready to be used
        /// </summary>
        public Action<string> SceneLoaded { get; set; }

        /// <summary>
        /// Fire this event when the scene is completely unloaded
        /// </summary>
        public Action<string> SceneUnloaded { get; set; }
    }

    /// <summary>
    /// /// Levels specifications : <br></br>
    /// * Level1 : Load the scene in the background and unload the current scene<br></br>
    /// * Level2 : Load the scene on the foreground and keep the current scene loaded<br></br>
    /// </summary>
    public enum SceneLevel
    {
        Level1,
        Level2
    }
}