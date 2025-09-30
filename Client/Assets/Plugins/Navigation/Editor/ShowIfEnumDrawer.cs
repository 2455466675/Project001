#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;

        // 获取目标对象
        object target = property.serializedObject.targetObject;

        // 通过反射获取枚举字段的值
        FieldInfo enumField = target.GetType().GetField(showIf.enumFieldName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (enumField != null)
        {
            object enumValue = enumField.GetValue(target);

            // 检查枚举值是否匹配
            if (enumValue.Equals(showIf.enumValue))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;

        object target = property.serializedObject.targetObject;
        FieldInfo enumField = target.GetType().GetField(showIf.enumFieldName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (enumField != null)
        {
            object enumValue = enumField.GetValue(target);

            if (enumValue.Equals(showIf.enumValue))
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return 0f; // 隐藏时高度为0
            }
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
