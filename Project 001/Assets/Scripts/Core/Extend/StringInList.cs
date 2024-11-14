using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class StringInList : PropertyAttribute
{
    public delegate string[] GetStringList();

    public string[] List { get; private set; }

    public StringInList(params string[] list)
    {
        List = list;
    }

    public StringInList(Type type, string methodName)
    {
        MethodInfo method = type.GetMethod(methodName);
        if (method != null)
        {
            try
            {
                List = method.Invoke(null, null) as string[];
                return;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return;
            }
        }

        Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
    }
}

