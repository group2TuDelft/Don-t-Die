namespace Crosstales.FontAwesome.Util
{
    /// <summary>Collected editor constants of very general utility for the asset.</summary>
    public static class Constants
    {

        #region Constant variables

        /// <summary>Is HD-version?</summary>
        public static readonly bool isHD = false;

        /// <summary>Name of the asset.</summary>
        public const string ASSET_NAME = "Icons - FontAwesome";

        /// <summary>Version of the asset.</summary>
        public const string ASSET_VERSION = "1.1.0";

        /// <summary>Build number of the asset.</summary>
        public const int ASSET_BUILD = 110;

        /// <summary>Create date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CREATED = new System.DateTime(2017, 11, 3);

        /// <summary>Change date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CHANGED = new System.DateTime(2017, 11, 29);

        /// <summary>Author of the asset.</summary>
        public const string ASSET_AUTHOR = "crosstales LLC";

        /// <summary>URL of the asset author.</summary>
        public const string ASSET_AUTHOR_URL = "https://www.crosstales.com";

        /// <summary>URL of the crosstales assets in UAS.</summary>
        public const string ASSET_CT_URL = "https://www.assetstore.unity3d.com/#!/list/42213-crosstales?aid=1011lNGT&pubref=" + ASSET_NAME; // crosstales list

        /// <summary>URL of the PRO asset in UAS.</summary>
        public const string ASSET_HD_URL = "https://www.assetstore.unity3d.com/#!/content/98720?aid=1011lNGT&pubref=" + ASSET_NAME;

        /// <summary>URL for update-checks of the asset</summary>
        public const string ASSET_UPDATE_CHECK_URL = "https://www.crosstales.com/media/assets/fa_versions.txt";

        /// <summary>Contact to the owner of the asset.</summary>
        public const string ASSET_CONTACT = "icons@crosstales.com";


        // Keys for the configuration of the asset
        public const string KEY_PREFIX = "CT_FONTAWESOME_CFG_";
        public const string KEY_UPDATE_CHECK = KEY_PREFIX + "UPDATE_CHECK";
        public const string KEY_UPDATE_OPEN_UAS = KEY_PREFIX + "UPDATE_OPEN_UAS";
        public const string KEY_REMINDER_CHECK = KEY_PREFIX + "REMINDER_CHECK";
        public const string KEY_TELEMETRY = KEY_PREFIX + "TELEMETRY";

        public const string KEY_UPDATE_DATE = KEY_PREFIX + "UPDATE_DATE";

        public const string KEY_REMINDER_DATE = KEY_PREFIX + "REMINDER_DATE";
        public const string KEY_REMINDER_COUNT = KEY_PREFIX + "REMINDER_COUNT";

        public const string KEY_TELEMETRY_DATE = KEY_PREFIX + "TELEMETRY_DATE";

        // Default values
        public const bool DEFAULT_UPDATE_CHECK = true;
        public const bool DEFAULT_UPDATE_OPEN_UAS = false;
        public const bool DEFAULT_REMINDER_CHECK = true;
        public const bool DEFAULT_TELEMETRY = true;

        #endregion


        #region Properties

        /// <summary>Returns the URL of the asset in UAS.</summary>
        /// <returns>The URL of the asset in UAS.</returns>
        public static string ASSET_URL
        {
            get
            {

                if (isHD)
                {
                    return Util.Constants.ASSET_HD_URL;
                }
                else
                {
                    return "https://www.assetstore.unity3d.com/#!/content/98719?aid=1011lNGT&pubref=" + Util.Constants.ASSET_NAME;
                }
            }
        }

        /// <summary>Returns the UID of the asset.</summary>
        /// <returns>The UID of the asset.</returns>
        public static System.Guid ASSET_UID
        {
            get
            {
                if (isHD)
                {
                    return new System.Guid("7bef7cfc-4649-46f5-a733-80ca59e03eb2");
                }
                else
                {
                    return new System.Guid("d3b51b18-2e62-43dc-9707-3cdd378c70e5");
                }
            }
        }

        #endregion

    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)