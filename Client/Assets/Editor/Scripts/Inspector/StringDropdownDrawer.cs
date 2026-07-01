using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringDropdownAttribute))]
public class StringDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use StringDropdown with string fields only.");
            return;
        }

        StringDropdownAttribute dropdown = (StringDropdownAttribute)attribute;
        string[] options = dropdown.options;

        // 找到当前值的索引
        string currentValue = property.stringValue;
        int selectedIndex = System.Array.IndexOf(options, currentValue);
        if (selectedIndex < 0) selectedIndex = 0;

        // 显示下拉菜单
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);

        // 设置新值
        if (selectedIndex >= 0 && selectedIndex < options.Length)
        {
            property.stringValue = options[selectedIndex];
        }
    }
}