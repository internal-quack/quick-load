#if UNITY_EDITOR

using UnityEditor;
using UnityEngine.UIElements;

namespace QuickLoad.Editor
{
    internal static class QuickLoadSettingsProvider
    {
        [SettingsProvider]
        internal static SettingsProvider CreateProvider()
        {
            SettingsProvider settingsProvider = new SettingsProvider(Constants.SettingsPath, SettingsScope.Project)
            {
                label = Constants.LabelName,
                activateHandler = OnActivate
            };

            return settingsProvider;
        }

        static void OnActivate(string searchContext, VisualElement rootElement)
        {
            QuickLoadConfig config = QuickLoadConfig.GetConfig();

            if (config == null)
            {
                HelpBox missingConfigHelpBox = new HelpBox(
                    string.Format(Constants.MissingConfigMessage, Constants.ConfigPath),
                    HelpBoxMessageType.Error);

                rootElement.Add(missingConfigHelpBox);
                return;
            }

            QuickLoadWindowBuilder sectionBuilder = new QuickLoadWindowBuilder(config);
            VisualElement section = sectionBuilder.Build();

            rootElement.style.marginTop = 8;
            rootElement.style.marginLeft = 8;
            rootElement.style.marginRight = 8;
            rootElement.Add(section);
        }
    }
}

#endif