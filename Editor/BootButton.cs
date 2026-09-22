using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace QuickLoad.Editor
{
    internal static class BootButton
    {
        const string ElementId = "QuickLoad/Play Button";
        const string ContentTooltip = "Custom Editor Loader\nPlay";
        const string IconName = "PlayButton";

        [MainToolbarElement(ElementId, defaultDockPosition = MainToolbarDockPosition.Middle, defaultDockIndex = 0)]
        static MainToolbarElement CreateBootButton()
        {
            Texture2D icon = EditorGUIUtility.IconContent(IconName).image as Texture2D;
            return new MainToolbarButton(new MainToolbarContent(icon, ContentTooltip), OnButtonClicked);
        }

        static void OnButtonClicked()
        {
            QuickLoadLifecycle.StartPlayMode();
        }
    }
}