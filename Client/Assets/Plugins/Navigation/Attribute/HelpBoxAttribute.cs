using System;
using UnityEngine;

public enum HelpBoxType
{
    None,    // 普通文本框（无图标）
    Info,    // 信息图标
    Warning, // 警告图标
    Error    // 错误图标
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class HelpBoxAttribute : PropertyAttribute
{
    public string Text { get; }
    public HelpBoxType Type { get; }

    public HelpBoxAttribute(string text, HelpBoxType type = HelpBoxType.Info)
    {
        Text = text;
        Type = type;
    }
}
