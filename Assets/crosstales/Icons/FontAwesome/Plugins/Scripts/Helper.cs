using UnityEngine;

namespace Crosstales.FontAwesome.Util
{
    /// <summary>Various helper functions.</summary>
    public static class Helper
    {

        #region Variables

        private static readonly System.Text.RegularExpressions.Regex lineEndingsRegex = new System.Text.RegularExpressions.Regex(@"\r\n|\r|\n");

        #endregion


        #region Static properties

        /// <summary>Checks if an Internet connection is available.</summary>
        /// <returns>True if an Internet connection is available.</returns>
        public static bool isInternetAvailable
        {
            get
            {
#if CT_OC
                return OnlineCheck.OnlineCheck.isInternetAvailable;
#else
                return Application.internetReachability != NetworkReachability.NotReachable;
#endif
            }
        }

        /// <summary>Checks if the current platform is Windows.</summary>
        /// <returns>True if the current platform is Windows.</returns>
        public static bool isWindowsPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
            }
        }

        /// <summary>Checks if the current platform is macOS.</summary>
        /// <returns>True if the current platform is macOS.</returns>
        public static bool isMacOSPlatform
        {
            get
            {
#if UNITY_5_4_OR_NEWER
                return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor;
#else
                return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer;
#endif
            }
        }

        /// <summary>Checks if the current platform is Linux.</summary>
        /// <returns>True if the current platform is Linux.</returns>
        public static bool isLinuxPlatform
        {
            get
            {
#if UNITY_5_5_OR_NEWER
                    return Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor;
#else
                return Application.platform == RuntimePlatform.LinuxPlayer;
#endif
            }
        }

        /// <summary>Checks if the current platform is Android.</summary>
        /// <returns>True if the current platform is Android.</returns>
        public static bool isAndroidPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.Android;
            }
        }

        /// <summary>Checks if the current platform is iOS.</summary>
        /// <returns>True if the current platform is iOS.</returns>
        public static bool isIOSPlatform
        {
            get
            {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS;
#else
                return Application.platform == RuntimePlatform.IPhonePlayer;
#endif
            }
        }

        /// <summary>Checks if the current platform is WSA.</summary>
        /// <returns>True if the current platform is WSA.</returns>
        public static bool isWSAPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.WSAPlayerARM ||
                    Application.platform == RuntimePlatform.WSAPlayerX86 ||
                    Application.platform == RuntimePlatform.WSAPlayerX64 ||
#if !UNITY_5_4_OR_NEWER
                    Application.platform == RuntimePlatform.WP8Player ||
#endif
#if !UNITY_5_5_OR_NEWER
                    Application.platform == RuntimePlatform.XBOX360 ||
#endif
                    Application.platform == RuntimePlatform.XboxOne;
            }
        }

        /// <summary>Checks if the current platform is WebGL.</summary>
        /// <returns>True if the current platform is WebGL.</returns>
        public static bool isWebGLPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.WebGLPlayer;
            }
        }

        /// <summary>Checks if we are inside the Editor.</summary>
        /// <returns>True if we are inside the Editor.</returns>
        public static bool isEditor
        {
            get
            {
#if UNITY_5_5_OR_NEWER
                return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor;
#else
                return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor;
#endif
            }
        }

        /// <summary>Checks if we are in Editor mode.</summary>
        /// <returns>True if in Editor mode.</returns>
        public static bool isEditorMode
        {
            get
            {
                return isEditor && !Application.isPlaying;
            }
        }

        #endregion


        #region Static methods

#if !UNITY_WSA
        /// <summary>HTTPS-certification callback.</summary>
        public static bool RemoteCertificateValidationCallback(System.Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;

#if UNITY_5_4_OR_NEWER
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = System.Security.Cryptography.X509Certificates.X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new System.TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = System.Security.Cryptography.X509Certificates.X509VerificationFlags.AllFlags;

                        isOk = chain.Build((System.Security.Cryptography.X509Certificates.X509Certificate2)certificate);
                    }
                }
            }
#endif

            return isOk;
        }
#endif

        /// <summary>Split the given text to lines and return it as list.</summary>
        /// <param name="text">Complete text fragment</param>
        /// <param name="ignoreCommentedLines">Ignore commente lines (default: true, optional)</param>
        /// <param name="skipHeaderLines">Number of skipped header lines (default: 0, optional)</param>
        /// <param name="skipFooterLines">Number of skipped footer lines (default: 0, optional)</param>
        /// <returns>Splitted lines as array</returns>
        public static System.Collections.Generic.List<string> SplitStringToLines(string text, bool ignoreCommentedLines = true, int skipHeaderLines = 0, int skipFooterLines = 0)
        {
            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>(100);

            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning("Parameter 'text' is null or empty!" + System.Environment.NewLine + "=> 'SplitStringToLines()' will return an empty string list.");
            }
            else
            {
                string[] lines = lineEndingsRegex.Split(text);

                for (int ii = 0; ii < lines.Length; ii++)
                {
                    if (ii + 1 > skipHeaderLines && ii < lines.Length - skipFooterLines)
                    {
                        if (!string.IsNullOrEmpty(lines[ii]))
                        {
                            if (ignoreCommentedLines)
                            {
                                if (!lines[ii].StartsWith("#", System.StringComparison.OrdinalIgnoreCase))
                                { //valid and not disabled line?
                                    result.Add(lines[ii]);
                                }
                            }
                            else
                            {
                                result.Add(lines[ii]);
                            }
                        }
                    }
                }
            }
            
            return result;
        }

        #endregion

    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)