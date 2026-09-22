namespace QuickLoad
{
    public interface INetworkLoader
    {
        void ConnectAsClient();
        void ConnectAsServer();
        void ApplyProtocol();
    }
}