using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QuickLoad
{
    internal static class QuickLoadSceneLoader
    {
        internal static void LoadScenes(IReadOnlyList<string> scenePaths, string initScenePath)
        {
            List<string> pathsToLoad = FilterPaths(scenePaths, initScenePath);

            if (pathsToLoad.Count == 0)
                return;

            LoadSequence(pathsToLoad);
        }

        internal static void LoadSceneAsync(string scenePath, Action onComplete = null)
        {
            LoadSceneParameters initParameters = new LoadSceneParameters(LoadSceneMode.Single);
            AsyncOperation initOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, initParameters);

            initOperation.completed += _ =>
            {
                onComplete?.Invoke();
            };
        }

        static void LoadSequence(List<string> scenePaths)
        {
            LoadSceneParameters singleParameters = new LoadSceneParameters(LoadSceneMode.Single);
            AsyncOperation firstOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(scenePaths[0], singleParameters);

            firstOperation.completed += _ => LoadRemaining(scenePaths);
        }

        static void LoadRemaining(List<string> scenePaths)
        {
            LoadSceneParameters additiveParameters = new LoadSceneParameters(LoadSceneMode.Additive);

            for (int i = 1; i < scenePaths.Count; i++)
                EditorSceneManager.LoadSceneAsyncInPlayMode(scenePaths[i], additiveParameters);
        }

        static List<string> FilterPaths(IReadOnlyList<string> scenePaths, string excludedScenePath)
        {
            List<string> filteredPaths = new List<string>();

            if (scenePaths == null)
                return filteredPaths;

            for (int i = 0; i < scenePaths.Count; i++)
            {
                string scenePath = scenePaths[i];

                if (string.IsNullOrEmpty(scenePath) || scenePath == excludedScenePath)
                    continue;

                filteredPaths.Add(scenePath);
            }

            return filteredPaths;
        }
    }
}