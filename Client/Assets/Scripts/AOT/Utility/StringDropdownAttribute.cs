using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringDropdownAttribute : PropertyAttribute
{
    public string[] options;

    public StringDropdownAttribute(params string[] options)
    {
        this.options = options;
    }
}
