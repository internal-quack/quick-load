namespace QuickLoad
{
    internal static class Constants
    {
        // Paths
        internal const string ConfigPath = "QuickLoad/Config";
        
        // Settings
        internal const string LabelName = "QuickLoad";
        internal const string SettingsPath = "Project/QuickLoad";

        #region Settings Window

            // Sections
            internal const string EditorTitle = "Editor";
            internal const string OptionsSection = "Options";
            internal const string ScenesSection = "Scenes";
            internal const string NetworkSection = "Network";

            // Fields
            internal const string InitSceneLabel = "Init Scene";
            internal const string AutomaticSceneSaveLabel = "Automatic Scene Save";
            internal const string EnableLocalNetworkLabel = "Enable Local Network";
            internal const string NetworkLoaderLabel = "Network Loader";

            // Tooltips
            internal const string AutomaticSceneSaveTooltip = "Saves all open scenes without asking before entering play mode.";
            internal const string EnableLocalNetworkTooltip = "Editor only. Skips the Steam lobby and connects locally.";

            // Undo
            internal const string UndoInitScene = "Change Init Scene";
            internal const string UndoAutomaticSceneSave = "Change Automatic Scene Save";
            internal const string UndoEnableLocalNetwork = "Change Enable Local Network";

            // Messages
            internal const string MissingConfigMessage = "QuickLoad config asset was not found in Resources/{0}.";
            
            // Layout
            internal const float LabelWidth = 160f;
            internal const float SectionTopMargin = 10f;
            internal const float SectionBottomMargin = 2f;
            internal const float SectionLineAlpha = 0.15f;

        #endregion

        #region Service

            // Messages
            internal const string InitScenePathNotSetMessage = "Init scene path is not set, go to project settings and select initialize scene";

        #endregion
    }
}