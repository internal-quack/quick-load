#if UNITY_EDITOR
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
#if UNITY_EDITOR
                return !CurrentPlayer.IsMainEditor;
#else
                return false;
#endif
            }
        }
    }
}