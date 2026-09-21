using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace QuickLoad.Drawers.Interface {
    // Only DataTypes/Structures supported
    // Only first layer research struct support
    
    [InitializeOnLoad]
    internal class InterfaceInitializer : UnityEditor.Editor {
        static InterfaceInitializer() {
            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.projectChanged += OnSelectionChanged;
        }
        
        [InitializeOnLoadMethod]
        static async void OnSelectionChanged() {
            // Delay between actions to provide correct selection
            await Task.Delay(100);

            if (Selection.activeObject is not ScriptableObject scriptableObject)
                return;
            
            CheckScriptableObject(scriptableObject);
        }

        static void CheckScriptableObject(ScriptableObject scriptableObject) {
            const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            
            var scType = scriptableObject.GetType();
            foreach (var field in scType.GetFields(Flags)) {
                // TODO: support class types
                // TODO: loop layers of structs more than one. Recursion
                
                var fieldType = field.FieldType;
                if (IsStruct(fieldType)) {
                    var structInstance = field.GetValue(scriptableObject);
                    
                    foreach (var structField in fieldType.GetFields(Flags)) {
                        if(!structField.FieldType.IsInterface)
                            continue;

                        CreateInterfaceDefaultReference(field, scriptableObject, structField, structInstance);
                    }
                }
                
                if(!field.FieldType.IsInterface)
                    continue;
                
                CreateInterfaceDefaultReference(field, scriptableObject, field, scriptableObject);
            }
        }

        
        static void CreateInterfaceDefaultReference(FieldInfo baseField, object baseInstance, FieldInfo interfaceField, object structInstance) {
            if (interfaceField.GetValue(structInstance) != null) 
                return;
            
            var interfaceTypes = InterfaceUtility.GetDerivedTypes(interfaceField.FieldType, false).
                Where(f => ContainsConstructor(f, true));
             
            var firstType = interfaceTypes.FirstOrDefault();
            if (firstType == null) {
                Debug.LogWarning($"Create even one class where derived from {interfaceField.FieldType.Name} with default constructor to allow draw the interface");
                return;
            }
            
            var newAbilityInstance = Activator.CreateInstance(firstType);
            interfaceField.SetValue(structInstance, newAbilityInstance);

            if (object.Equals(baseInstance, structInstance))
                return;
            
            baseField.SetValue(baseInstance, structInstance);
        }
        
         static bool ContainsConstructor(Type type, bool onlyDefault = false) =>
            type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).
                Where(constr => !onlyDefault || constr.GetParameters().Length == 0).ToList().Count > 0;
         
         static bool IsStruct(Type type) => type.IsValueType && !type.IsEnum;    
    }
}