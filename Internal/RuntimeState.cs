using System;
using System.IO;
using UnityEngine;

namespace QuickLoad
{
    /// <summary>
    /// Manages transient QuickLoad session state shared across all editor instances.
    /// 
    /// WHY A FILE IN THE SYSTEM TEMP DIRECTORY:
    /// In Unity Multiplayer Play Mode (MPPM), each virtual player (clone) runs as a completely separate
    /// OS process with isolated RAM and independent C# object state.
    /// 
    /// Writing state to a temp file in the system temp directory:
    /// 1. Provides instant inter-process synchronization (main editor and all MPPM clones access the same data).
    /// 2. Survives full Unity Domain Reload when entering Play Mode.
    /// 3. Does not mutate project assets (ScriptableObjects) or generate dirty Git changes.
    /// </summary>
    public static class RuntimeState
    {
        static readonly string SessionFilePath = Path.Combine(Path.GetTempPath(), Constants.SessionFileName);

        public static bool IsRunningWithLoader => File.Exists(SessionFilePath);

        public static string[] ScenesToLoadPath
        {
            get
            {
                if (!IsRunningWithLoader)
                {
                    return Array.Empty<string>();
                }

                try
                {
                    return File.ReadAllLines(SessionFilePath);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(string.Format(Constants.FailedToReadSessionFileMessage, ex.Message));
                    return Array.Empty<string>();
                }
            }
        }

        internal static void SetSession(string[] scenes)
        {
            try
            {
                string directory = Path.GetDirectoryName(SessionFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllLines(SessionFilePath, scenes ?? Array.Empty<string>());
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Constants.FailedToWriteSessionFileMessage, ex.Message));
            }
        }

        internal static void ClearSession()
        {
            try
            {
                if (File.Exists(SessionFilePath))
                {
                    File.Delete(SessionFilePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format(Constants.FailedToDeleteSessionFileMessage, ex.Message));
            }
        }
    }
}
