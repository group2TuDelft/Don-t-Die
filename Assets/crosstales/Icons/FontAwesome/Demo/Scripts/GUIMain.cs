using UnityEngine;
using UnityEngine.UI;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using Crosstales.FontAwesome.Util;

namespace Crosstales.FontAwesome.Demo
{
    /// <summary>Main GUI component for all demo scenes.</summary>
    //[HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_g_u_i_main.html")] //TODO set correct URL
    public class GUIMain : MonoBehaviour
    {

        #region Variables

        public Text Name;
        public Text Version;
        public Text Scene;

        #endregion


        #region MonoBehaviour methods

        public void Start()
        {
            if (Name != null)
            {
                Name.text = Constants.ASSET_NAME;
            }

            if (Version != null)
            {
                Version.text = Constants.ASSET_VERSION;
            }

            if (Scene != null)
            {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                Scene.text = SceneManager.GetActiveScene().name;
#else
                Scene.text = Application.loadedLevelName;
#endif
            }
        }

        #endregion


        #region Public methods

        public void OpenAssetURL()
        {
            Application.OpenURL(Constants.ASSET_CT_URL);
        }

        public void OpenAssetHDURL()
        {
            Application.OpenURL(Constants.ASSET_HD_URL);
        }

        public void OpenCTURL()
        {
            Application.OpenURL(Constants.ASSET_AUTHOR_URL);
        }

        public void Quit()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }

        #endregion
    }
}
// Copyright 2016-2017 www.crosstales.com