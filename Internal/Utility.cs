using System.Reflection;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace QuickLoad
{
    internal static class Utility
    {
        internal static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method!.Invoke(null, null);
        }

        internal static string[] GetLoadedScenePaths()
        {
            List<string> scenePaths = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (!scene.isLoaded)
                    continue;

                scenePaths.Add(scene.path);
            }

            return scenePaths.ToArray();
        }
    }
}