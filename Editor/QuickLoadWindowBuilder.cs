#if UNITY_EDITOR

using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;

namespace QuickLoad.Editor
{
    internal class QuickLoadWindowBuilder
    {
        readonly QuickLoadConfig _config;
        readonly SerializedObject _serializedConfig;

        VisualElement _networkLoaderContainer;

        internal QuickLoadWindowBuilder(QuickLoadConfig config)
        {
            _config = config;
            _serializedConfig = new SerializedObject(config);
        }

        internal VisualElement Build()
        {
            VisualElement sectionRoot = new VisualElement();

            sectionRoot.Add(CreateTitleLabel());

            sectionRoot.Add(CreateSectionLabel(Constants.OptionsSection));
            sectionRoot.Add(CreateAutomaticSceneSaveToggle());
            sectionRoot.Add(CreateLocalNetworkToggle());

            sectionRoot.Add(CreateSectionLabel(Constants.ScenesSection));
            sectionRoot.Add(CreateInitSceneField());

            _networkLoaderContainer = CreateNetworkLoaderContainer();
            sectionRoot.Add(_networkLoaderContainer);

            UpdateNetworkLoaderVisibility(_config.EnableLocalNetwork);

            return sectionRoot;
        }

        Label CreateTitleLabel()
        {
            return new Label(Constants.EditorTitle)
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 4 }
            };
        }

        Label CreateSectionLabel(string text)
        {
            return new Label(text)
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginTop = Constants.SectionTopMargin,
                    marginBottom = Constants.SectionBottomMargin,
                    paddingBottom = 2,
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(1f, 1f, 1f, Constants.SectionLineAlpha)
                }
            };
        }

        ObjectField CreateInitSceneField()
        {
            ObjectField initSceneField = new ObjectField(Constants.InitSceneLabel)
            {
                objectType = typeof(SceneAsset),
                allowSceneObjects = false,
                value = LoadSceneAsset(_config.InitScenePath)
            };

            initSceneField.RegisterValueChangedCallback(OnInitSceneChanged);
            AlignField(initSceneField);

            return initSceneField;
        }

        Toggle CreateAutomaticSceneSaveToggle()
        {
            Toggle automaticSceneSaveToggle = new Toggle(Constants.AutomaticSceneSaveLabel)
            {
                value = _config.AutomaticSceneSave,
                tooltip = Constants.AutomaticSceneSaveTooltip
            };

            automaticSceneSaveToggle.RegisterValueChangedCallback(OnAutomaticSceneSaveChanged);
            AlignField(automaticSceneSaveToggle);

            return automaticSceneSaveToggle;
        }

        Toggle CreateLocalNetworkToggle()
        {
            Toggle localNetworkToggle = new Toggle(Constants.EnableLocalNetworkLabel)
            {
                value = _config.EnableLocalNetwork,
                tooltip = Constants.EnableLocalNetworkTooltip
            };

            localNetworkToggle.RegisterValueChangedCallback(OnLocalNetworkChanged);
            AlignField(localNetworkToggle);

            return localNetworkToggle;
        }

        VisualElement CreateNetworkLoaderContainer()
        {
            VisualElement container = new VisualElement();
            container.Add(CreateSectionLabel(Constants.NetworkSection));

            SerializedProperty networkLoaderProperty = _serializedConfig.FindProperty(nameof(QuickLoadConfig.NetworkLoader));

            PropertyField networkLoaderField = new PropertyField(networkLoaderProperty, Constants.NetworkLoaderLabel);
            networkLoaderField.Bind(_serializedConfig);
            container.Add(networkLoaderField);

            return container;
        }

        void AlignField<T>(BaseField<T> field)
        {
            field.AddToClassList(BaseField<T>.alignedFieldUssClassName);
            field.labelElement.style.minWidth = Constants.LabelWidth;
        }

        void UpdateNetworkLoaderVisibility(bool isVisible)
        {
            _networkLoaderContainer.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        SceneAsset LoadSceneAsset(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                return null;

            return AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        }

        void OnInitSceneChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            Undo.RecordObject(_config, Constants.UndoInitScene);

            _config.InitScenePath = changeEvent.newValue == null
                ? string.Empty
                : AssetDatabase.GetAssetPath(changeEvent.newValue);

            EditorUtility.SetDirty(_config);
        }

        void OnAutomaticSceneSaveChanged(ChangeEvent<bool> changeEvent)
        {
            Undo.RecordObject(_config, Constants.UndoAutomaticSceneSave);
            _config.AutomaticSceneSave = changeEvent.newValue;
            EditorUtility.SetDirty(_config);
        }

        void OnLocalNetworkChanged(ChangeEvent<bool> changeEvent)
        {
            Undo.RecordObject(_config, Constants.UndoEnableLocalNetwork);
            _config.EnableLocalNetwork = changeEvent.newValue;
            EditorUtility.SetDirty(_config);

            UpdateNetworkLoaderVisibility(changeEvent.newValue);
        }
    }
}

#endif