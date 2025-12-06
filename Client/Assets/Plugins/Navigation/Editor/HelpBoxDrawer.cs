#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxDrawer : DecoratorDrawer
{
    private HelpBoxAttribute HelpBoxAttribute => (HelpBoxAttribute)attribute;

    public override float GetHeight()
    {
        var helpBoxAttribute = HelpBoxAttribute;
        if (helpBoxAttribute == null) return base.GetHeight();

        var helpBoxStyle = GUI.skin.GetStyle("helpbox");
        if (helpBoxStyle == null) return base.GetHeight();

        return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.Text), EditorGUIUtility.currentViewWidth) + 4);
    }

    public override void OnGUI(Rect position)
    {
        var helpBoxAttribute = HelpBoxAttribute;
        if (helpBoxAttribute == null) return;

        var messageType = MessageType.Info;
        switch (helpBoxAttribute.Type)
        {
            case HelpBoxType.Info:
                messageType = MessageType.Info;
                break;
            case HelpBoxType.Warning:
                messageType = MessageType.Warning;
                break;
            case HelpBoxType.Error:
                messageType = MessageType.Error;
                break;
            case HelpBoxType.None:
                messageType = MessageType.None;
                break;
        }

        EditorGUI.HelpBox(position, helpBoxAttribute.Text, messageType);
    }
}
#endif