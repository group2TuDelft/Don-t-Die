using UnityEngine;
using UnityEditor;

namespace Crosstales.FontAwesome.Util
{
    /// <summary>Editor configuration for the asset.</summary>
    [InitializeOnLoad]
    public static class Config
    {

        #region Variables

        /// <summary>Enable or disable update-checks for the asset.</summary>
        public static bool UPDATE_CHECK = Constants.DEFAULT_UPDATE_CHECK;

        /// <summary>Open the UAS-site when an update is found.</summary>
        public static bool UPDATE_OPEN_UAS = Constants.DEFAULT_UPDATE_OPEN_UAS;

        /// <summary>Enable or disable reminder-checks for the asset.</summary>
        public static bool REMINDER_CHECK = Constants.DEFAULT_REMINDER_CHECK;

        /// <summary>Enable or disable anonymous telemetry data.</summary>
        public static bool TELEMETRY = Constants.DEFAULT_TELEMETRY;

        /// <summary>Is the configuration loaded?</summary>
        public static bool isLoaded = false;

        #endregion

        
        #region Constructor

        static Config()
        {
            if (!isLoaded)
            {
                Load();
            }
        }

        #endregion


        #region Public static methods

        /// <summary>Resets all changable variables to their default value.</summary>
        public static void Reset()
        {
            UPDATE_CHECK = Constants.DEFAULT_UPDATE_CHECK;
            UPDATE_OPEN_UAS = Constants.DEFAULT_UPDATE_OPEN_UAS;
            REMINDER_CHECK = Constants.DEFAULT_REMINDER_CHECK;
            TELEMETRY = Constants.DEFAULT_TELEMETRY;
        }

        /// <summary>Loads the all changable variables.</summary>
        public static void Load()
        {
            if (CTPlayerPrefs.HasKey(Constants.KEY_UPDATE_CHECK))
            {
                UPDATE_CHECK = CTPlayerPrefs.GetBool(Constants.KEY_UPDATE_CHECK);
            }

            if (CTPlayerPrefs.HasKey(Constants.KEY_UPDATE_OPEN_UAS))
            {
                UPDATE_OPEN_UAS = CTPlayerPrefs.GetBool(Constants.KEY_UPDATE_OPEN_UAS);
            }

            if (CTPlayerPrefs.HasKey(Constants.KEY_REMINDER_CHECK))
            {
                REMINDER_CHECK = CTPlayerPrefs.GetBool(Constants.KEY_REMINDER_CHECK);
            }

            if (CTPlayerPrefs.HasKey(Constants.KEY_TELEMETRY))
            {
                TELEMETRY = CTPlayerPrefs.GetBool(Constants.KEY_TELEMETRY);
            }

            isLoaded = true;
        }

        /// <summary>Saves the all changable variables.</summary>
        public static void Save()
        {
            CTPlayerPrefs.SetBool(Constants.KEY_UPDATE_CHECK, UPDATE_CHECK);
            CTPlayerPrefs.SetBool(Constants.KEY_UPDATE_OPEN_UAS, UPDATE_OPEN_UAS);
            CTPlayerPrefs.SetBool(Constants.KEY_REMINDER_CHECK, REMINDER_CHECK);
            CTPlayerPrefs.SetBool(Constants.KEY_TELEMETRY, TELEMETRY);

            CTPlayerPrefs.Save();
        }

        #endregion
    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)