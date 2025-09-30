using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ShowIfEnumAttribute : PropertyAttribute
{
    public string enumFieldName;
    public object enumValue;

    public ShowIfEnumAttribute(string enumFieldName, object enumValue)
    {
        this.enumFieldName = enumFieldName;
        this.enumValue = enumValue;
    }
}