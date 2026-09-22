using UnityEditor;
using UnityEngine;

namespace QuickLoad
{
    [InitializeOnLoad]
    internal static class QuickLoadHandler
    {
        static QuickLoadHandler()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (!RuntimeState.IsRunningWithLoader) 
                return;
            
            QuickLoadConfig config = QuickLoadConfig.GetConfig();
            
            switch (state) 
            {
                case PlayModeStateChange.EnteredEditMode: 
                    QuickLoadLifecycle.ExitPlayMode();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    QuickLoadLifecycle.EnterPlayMode(config);
                    break;
            }
        }
    }
}