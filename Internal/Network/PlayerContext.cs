#if QUICKLOAD_MPPM
using Unity.Multiplayer.PlayMode;
#endif

namespace QuickLoad
{
    public static class PlayerContext
    {
        public static bool IsCloneEditor
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
    }
}