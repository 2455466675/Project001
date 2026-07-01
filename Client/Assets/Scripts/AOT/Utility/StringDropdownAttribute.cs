using System;
using UnityEngine;

/// <summary>
/// 字符串下拉菜单
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringDropdownAttribute : PropertyAttribute
{
    public string[] options;

    public StringDropdownAttribute(params string[] options)
    {
        this.options = options;
    }
}