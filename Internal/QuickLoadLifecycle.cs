using UnityEditor.SceneManagement;
using UnityEngine.Assertions;
using UnityEditor;

namespace QuickLoad
{
    internal static class QuickLoadLifecycle
    {
        internal static void StartPlayMode()
        {
            QuickLoadConfig config = QuickLoadConfig.GetConfig();
         
            Assert.IsFalse(string.IsNullOrEmpty(config.InitScenePath), Constants.InitScenePathNotSetMessage);
            
            if (config.AutomaticSceneSave)
            {
                if (!EditorSceneManager.SaveOpenScenes())
                {
                    return;
                }
            }
            else if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }
            
            RuntimeState.SetSession(Utility.GetLoadedScenePaths());
            
            EditorSceneManager.OpenScene(config.InitScenePath);
            EditorApplication.EnterPlaymode();
        }
        
        internal static void ExitPlayMode()
        {
            string[] scenesToLoad = RuntimeState.ScenesToLoadPath;
            if (scenesToLoad == null || scenesToLoad.Length == 0)
            {
                RuntimeState.ClearSession();
                return;
            }
            
            for (int i = 0; i < scenesToLoad.Length; i++)
            {
                OpenSceneMode mode = i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive;
                        
                EditorSceneManager.OpenScene(scenesToLoad[i], mode);
            }

            RuntimeState.ClearSession();
        }
        
        internal static void EnterPlayMode(QuickLoadConfig config)
        {
            if (PlayerContext.IsCloneEditor)
            {
                QuickLoadSceneLoader.LoadSceneAsync(config.InitScenePath, () =>
                {
                    Utility.ClearConsole();
                    QuickLoadSceneResolver.ResolveSceneLoading(config);
                });

                return;
            }
                    
            QuickLoadSceneResolver.ResolveSceneLoading(config);
        }
    }
}