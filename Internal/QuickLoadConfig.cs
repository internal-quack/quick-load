using QuickLoad.Drawers;
using Unity.Collections;
using UnityEngine;

namespace QuickLoad
{
    [CreateAssetMenu(menuName = "QuickLoad/Config")]
    public class QuickLoadConfig : ScriptableObject
    {
        [Header("Editor")]
        [SerializeField, ReadOnly] internal bool EnableLocalNetwork;
        [SerializeField, ReadOnly] internal bool AutomaticSceneSave;
        [SerializeField, ReadOnly] internal bool IsRunningWithLoader;

        [SerializeField, ReadOnly] internal string InitScenePath;
        [SerializeField, ReadOnly] internal string[] ScenesToLoadPath;

        [SerializeField, SerializeReference, ShowInterface] 
        internal INetworkLoader NetworkLoader;
        
        static QuickLoadConfig Config { get; set; }
        internal static QuickLoadConfig GetConfig() => Config == null ? Resources.Load<QuickLoadConfig>(Constants.ConfigPath) : Config;
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeConfig()
        {
            Config = GetConfig();
        }

        void Reset()
        {
            NetworkLoader = null;
            EnableLocalNetwork = false;
            AutomaticSceneSave = true;
        }
    }
}