using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringInList))]
public class StringInListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 获取 `StringInList` 属性实例
        StringInList stringInList = (StringInList)attribute;

        // 确保该字段是字符串类型
        if (property.propertyType == SerializedPropertyType.String)
        {
            // 获取当前的字符串列表
            string[] options = stringInList.List;
            if (options != null && options.Length > 0)
            {
                // 找到当前选择的索引
                int selectedIndex = Mathf.Max(0, System.Array.IndexOf(options, property.stringValue));

                // 绘制下拉菜单
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);

                // 更新字段值
                property.stringValue = options[selectedIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "No options available");
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use [StringInList] with string.");
        }
    }
}