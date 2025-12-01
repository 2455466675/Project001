using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(AbstractTypeAttribute))]
public class AbstractTypeDrawer : PropertyDrawer
{
    private Dictionary<string, Type> typeMap;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    { 
        AbstractTypeAttribute abstractTypeAttribute = (AbstractTypeAttribute)attribute;

        var singleLineHeight = EditorGUIUtility.singleLineHeight;
        var typeRect = new Rect(position.x, position.y, position.width, singleLineHeight);
        var contentRect = new Rect(position.x, position.y + singleLineHeight, position.width, position.height - singleLineHeight);

        EditorGUI.BeginProperty(position, label, property);

        var typeName = property.managedReferenceFullTypename;
        var displayName = GetShortTypeName(typeName);

        if (EditorGUI.DropdownButton(typeRect, new GUIContent(string.IsNullOrEmpty(displayName) ? "Select Type" : displayName), FocusType.Keyboard))
        {
            BuildTypeMap(abstractTypeAttribute.type);

            var menu = new GenericMenu();
            if (typeMap == null || typeMap.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("No Type available"));
                menu.ShowAsContext();
                return;
            }

            foreach (var item in typeMap)
            {
                var name = item.Key;
                var type = item.Value;
                menu.AddItem(new GUIContent(name), type.FullName == typeName, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(contentRect, property, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;
    }

    private void BuildTypeMap(Type baseType)
    {
        typeMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>{
                try
                {
                return a.GetTypes();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToDictionary(t => ObjectNames.NicifyVariableName(t.Name), t => t);
    }

    private string GetShortTypeName(string fullTypeName)
    {
        if (string.IsNullOrEmpty(fullTypeName))
        {
            return string.Empty;
        }

        var parts = fullTypeName.Split(' ');
        return parts.Length > 1 ? parts[1].Split('.').Last() : fullTypeName;
    }
}
