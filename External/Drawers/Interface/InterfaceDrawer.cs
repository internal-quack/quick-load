using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickLoad.Drawers.Interface {
    //TODO: Make custom serializer for everyone type
    //TODO: Including struct/interface
    //TODO: Current is showing and give ability to show interface 
    //[CustomPropertyDrawer(typeof(object), true)]
    [CustomPropertyDrawer(typeof(ShowInterfaceAttribute), true)]
    internal class InterfaceDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.propertyType == SerializedPropertyType.ManagedReference
                && property.managedReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            float height = 0f;
            if (property.hasVisibleChildren) {
                var subProperty = property.Copy();
                var nextElement = property.GetEndProperty();

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                while (subProperty.NextVisible(true) && !SerializedProperty.EqualContents(subProperty, nextElement)) {
                    height += EditorGUI.GetPropertyHeight(subProperty) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else {
                height = EditorGUI.GetPropertyHeight(property, true);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldType = fieldInfo.FieldType;

            if (!fieldType.IsInterface) {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (property.propertyType != SerializedPropertyType.ManagedReference) {
                EditorGUI.LabelField(position, $"{label.text}: Not a SerializeReference field");
                return;
            }

            var types = InterfaceUtility.GetDerivedTypes(fieldType, false).ToArray();

            if (!types.Any()) {
                EditorGUI.LabelField(position, $"{label.text}: No implementable types found");
                return;
            }

            var currentTypeName = property.managedReferenceValue?.GetType().FullName;

            var options = new[] { "None" }.Concat(types.Select(t => t.Name)).ToArray();
            int currentIndex = currentTypeName == null
                ? 0
                : Array.FindIndex(types, t => t.FullName == currentTypeName) + 1;

            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(popupRect, label.text, currentIndex, options);
            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.Update();
                property.managedReferenceValue = newIndex == 0
                    ? null
                    : Activator.CreateInstance(types[newIndex - 1]);
                property.serializedObject.ApplyModifiedProperties();
            }

            if (property.managedReferenceValue != null)
                ShowFields(property, position, true);
        }

        void ShowFields(SerializedProperty property, Rect position, bool indentFields = false) {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty iterator = property.Copy(); 
            SerializedProperty endProperty = property.GetEndProperty();

            if(indentFields)
                EditorGUI.indentLevel++;

            iterator.NextVisible(true);

            while (!SerializedProperty.EqualContents(iterator, endProperty))
            {
                position.height = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(position, iterator, true);
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                iterator.NextVisible(false);
            }

            if(indentFields)
                EditorGUI.indentLevel--;
        }
    }
}