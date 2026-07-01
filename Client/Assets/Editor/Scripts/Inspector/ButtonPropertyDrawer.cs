#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;

        // 获取按钮文本
        string buttonText = string.IsNullOrEmpty(buttonAttribute.ButtonText)
            ? ObjectNames.NicifyVariableName(property.name)
            : buttonAttribute.ButtonText;

        // 绘制按钮
        if (GUI.Button(position, buttonText))
        {
            // 获取目标对象
            object target = property.serializedObject.targetObject;

            // 通过反射调用方法
            MethodInfo method = target.GetType().GetMethod(property.name,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null)
            {
                method.Invoke(target, null);

                // 标记对象为已修改（用于撤销系统）
                if (target is UnityEngine.Object unityObject)
                {
                    EditorUtility.SetDirty(unityObject);
                }
            }
            else
            {
                Debug.LogWarning($"Method '{property.name}' not found on {target.GetType().Name}");
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
        return buttonAttribute.ButtonHeight;
    }
}
#endif
