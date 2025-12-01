using System;
using UnityEngine;

/// <summary>
/// ×Ö·û´®ÏÂÀ­²Ëµ¥
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
