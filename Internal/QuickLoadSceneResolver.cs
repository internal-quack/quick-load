using UnityEngine.Assertions;

namespace QuickLoad
{
    internal static class QuickLoadSceneResolver
    {
        internal static void ResolveSceneLoading(QuickLoadConfig config)
        {
            switch (config.EnableLocalNetwork)
            {
                case false:
                    QuickLoadSceneLoader.LoadScenes(RuntimeState.ScenesToLoadPath, config.InitScenePath);
                    break;
                        
                case true:
                    ResolveNetworkLoader(config.NetworkLoader);
                    break;
            }
        }
        
        static void ResolveNetworkLoader(INetworkLoader loader)
        {
            Assert.IsNotNull(loader, Constants.NetworkLoaderIsNullMessage);
         
            loader.ApplyProtocol();
            
            if (PlayerContext.IsClient)
            {
                loader.ConnectAsClient();
            }
            else if (!PlayerContext.IsAssigned || PlayerContext.IsHost)
            {
                loader.ConnectAsServer();
                loader.ConnectAsClient();
            }
        }
    }
}