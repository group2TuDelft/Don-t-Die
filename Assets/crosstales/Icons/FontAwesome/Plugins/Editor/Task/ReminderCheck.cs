using UnityEngine;
using UnityEditor;
using Crosstales.FontAwesome.Util;

namespace Crosstales.FontAwesome.Task
{
    /// <summary>Reminds the customer to create an UAS review.</summary>
    [InitializeOnLoad]
    public static class ReminderCheck
    {
        #region Constructor

        static ReminderCheck()
        {
            string lastDate = EditorPrefs.GetString(Constants.KEY_REMINDER_DATE);
            string date = System.DateTime.Now.ToString("yyyyMMdd"); // every day
            //string date = System.DateTime.Now.ToString("yyyyMMddHHmm"); // every minute (for tests)

            if (!date.Equals(lastDate))
            {
                int count = EditorPrefs.GetInt(Constants.KEY_REMINDER_COUNT) + 1;

                //if (count % 1 == 0) // for testing only
                if (count % 13 == 0 && Config.REMINDER_CHECK)
                {

                    int option = EditorUtility.DisplayDialogComplex(Constants.ASSET_NAME + " - Reminder",
                                "Please don't forget to rate " + Constants.ASSET_NAME + " or even better write a little review – it would be very much appreciated!",
                                "Yes, let's do it!",
                                "Not right now",
                                "Don't ask again!");

                    if (option == 0)
                    {
                        Application.OpenURL(Constants.ASSET_URL);
                        Config.REMINDER_CHECK = false;

                        GAApi.Event(typeof(ReminderCheck).Name, "rate", "count", count);
                    }
                    else if (option == 1)
                    {
                        // do nothing!
                        GAApi.Event(typeof(ReminderCheck).Name, "later", "count", count);
                    }
                    else
                    {
                        Config.REMINDER_CHECK = false;
                        GAApi.Event(typeof(ReminderCheck).Name, "never", "count", count);
                    }

                    Config.Save();
                }

                EditorPrefs.SetString(Constants.KEY_REMINDER_DATE, date);
                EditorPrefs.SetInt(Constants.KEY_REMINDER_COUNT, count);
            }
        }

        #endregion

    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)