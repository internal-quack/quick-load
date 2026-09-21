using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickLoad
{
    [InitializeOnLoad]
    public static class QuickLoadService 
    {
        static QuickLoadService()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            QuickLoadConfig config = QuickLoadConfig.GetConfig();

            if (PlayerContext.IsCloneEditor)
            {
                HandleClone(state, config);
                return;
            }

            if (!config.IsRunningWithLoader) 
                return;
            
            switch (state) 
            {
                case PlayModeStateChange.EnteredEditMode: 
                {
                    ExitPlayMode(config);
                }
                    break;
                case PlayModeStateChange.EnteredPlayMode: 
                {
                    QuickLoadSceneLoader.LoadScenes(config.ScenesToLoadPath, config.InitScenePath);
                }
                    break;
            }
        }

        static void HandleClone(PlayModeStateChange state, QuickLoadConfig config)
        {
            if (state != PlayModeStateChange.EnteredPlayMode)
                return;

            if (string.IsNullOrEmpty(config.InitScenePath))
                return;

            QuickLoadSceneLoader.LoadInitAndScenes(config.InitScenePath, GetLoadedScenePaths());
        }

        public static void StartPlayMode()
        {
            QuickLoadConfig config = QuickLoadConfig.GetConfig();
            if (string.IsNullOrEmpty(config.InitScenePath))
            {
                Debug.LogError(Constants.InitScenePathNotSetMessage);
                return;
            }
            
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
            
            config.IsRunningWithLoader = true;
            config.ScenesToLoadPath = GetLoadedScenePaths().ToArray();
                        
            EditorSceneManager.OpenScene(config.InitScenePath);
            EditorApplication.EnterPlaymode();
        }
        
        static void ExitPlayMode(QuickLoadConfig config)
        {
            if (config.ScenesToLoadPath == null || config.ScenesToLoadPath.Length == 0)
            {
                config.IsRunningWithLoader = false;
                return;
            }
            
            string[] scenes = config.ScenesToLoadPath;
            for (int i = 0; i < scenes.Length; i++)
            {
                OpenSceneMode mode = i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive;
                        
                EditorSceneManager.OpenScene(scenes[i], mode);
            }

            config.IsRunningWithLoader = false;
        }

        static List<string> GetLoadedScenePaths()
        {
            List<string> scenePaths = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (!scene.isLoaded)
                    continue;

                scenePaths.Add(scene.path);
            }

            return scenePaths;
        }
    }
}