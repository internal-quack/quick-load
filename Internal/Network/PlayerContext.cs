#if QUICKLOAD_MPPM
using Unity.Multiplayer.PlayMode;
#endif

namespace QuickLoad
{
    internal static class PlayerContext
    {
        internal static bool IsCloneEditor
        {
            get
            {
#if QUICKLOAD_MPPM
                return !CurrentPlayer.IsMainEditor;
#else
                return false;
#endif
            }
        }
        
        internal static bool IsAssigned => IsClient || IsHost;

        internal static bool IsHost
        {
            get
            {
#if QUICKLOAD_MPPM
                foreach (string tag in CurrentPlayer.Tags)
                {
                    if (string.Equals(tag, Constants.HostTag, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
#endif
                return false;
            }
        }
        
        internal static bool IsClient
        {
            get
            {
#if QUICKLOAD_MPPM
                foreach (string tag in CurrentPlayer.Tags)
                {
                    if (string.Equals(tag, Constants.ClientTag, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
#endif
                return false;
            }
        }
    }
}