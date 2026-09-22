using System;
using System.IO;
using PurrNet;
using PurrNet.Modules;
using PurrNet.Transports;
using QuickLoad;
using UnityEngine.SceneManagement;

namespace External.PurrNet
{
    [Serializable]
    public class PurrQuickLoad : INetworkLoader
    {
        public void ConnectAsClient()
        {
            NetworkManager.main.StartClient();
        }

        public void ConnectAsServer()
        {
            NetworkManager.main.StartHost();
            
            string targetScene = Path.GetFileNameWithoutExtension(RuntimeState.ScenesToLoadPath[0]);
            Load(targetScene);
        }

        public void ApplyProtocol()
        {
            NetworkManager.main.transport = 
                NetworkManager.main.GetComponent<UDPTransport>() ?? 
                NetworkManager.main.gameObject.AddComponent<UDPTransport>();
        }

        static void Load(string sceneName)
        {
            PurrSceneSettings settings = new PurrSceneSettings();
            settings.isPublic = true;
            settings.mode = LoadSceneMode.Single;

            NetworkManager.main.sceneModule.LoadSceneAsync(sceneName, settings);
        }
    }
}