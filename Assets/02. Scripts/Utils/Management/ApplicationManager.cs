using UnityEngine;

namespace Utils.Management
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        private bool _isApplicationPaused;

        private ApplicationManager()
        {
            _isApplicationPaused = false;
        }

        #region STATIC METHOD API

        /// <summary>
        /// Pause the running application.
        /// </summary>
        public static void OnApplicationPause()
        {
            if (Instance._isApplicationPaused)
            {
                Debug.LogWarning("<b>Application</b> has already been paused");

                return;
            }

            Time.timeScale = 0f;

            Instance._isApplicationPaused = true;

            Debug.Log("<b>Application</b> is paused");
        }

        /// <summary>
        /// Play the quiting application.
        /// </summary>
        public static void OnApplicationPlay()
        {
            if (Instance._isApplicationPaused == false)
            {
                Debug.LogWarning("<b>Application</b> is currently running");

                return;
            }

            Time.timeScale = 1f;

            Instance._isApplicationPaused = false;

            Debug.Log("<b>Application</b> is played");
        }

        /// <summary>
        /// Quits the player application.
        /// </summary>
        public static void OnApplicationQuit()
        {
            Debug.Log("<b>Game Application</b> is quited");

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

#else

            UnityEngine.Application.Quit();

#endif
        }

        #endregion

        #region STATIC PROPERTIES API

        public static bool IsApplicationPaused => Instance._isApplicationPaused;

        #endregion
    }
}
