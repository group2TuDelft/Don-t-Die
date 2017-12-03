using UnityEditor;
using Crosstales.FontAwesome.Util;

namespace Crosstales.FontAwesome.Task
{
    /// <summary>Gather some telemetry data for the asset.</summary>
    [InitializeOnLoad]
    public static class Telemetry
    {
        #region Constructor

        static Telemetry()
        {
            string lastDate = string.Empty;
            if (CTPlayerPrefs.HasKey(Constants.KEY_TELEMETRY_DATE))
            {
                lastDate = CTPlayerPrefs.GetString(Constants.KEY_TELEMETRY_DATE);
            }

            string date = System.DateTime.Now.ToString("yyyyMMdd"); // every day
            //string date = System.DateTime.Now.ToString("yyyyMMddHHmm"); // every minute (for tests)

            if (!date.Equals(lastDate))
            {
                GAApi.Event(typeof(Telemetry).Name, "Startup");

                CTPlayerPrefs.SetString(Constants.KEY_TELEMETRY_DATE, date);
            }
        }

        #endregion

    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)