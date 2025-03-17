using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.UI
{
    public class TestProxy
    {
        public void TestFun() 
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Debug.Log("assembly : " + assembly.FullName);
            Type[] types = assembly.GetTypes();
            foreach (Type t in types) 
            {
                var attr = t.GetCustomAttribute<GroupProxyAttribute>();
                if (attr != null) 
                {
                    MLog.Log("TestFun : ", t.FullName, attr.Define);
                    object o = Activator.CreateInstance(t);
                }
            }
        }
    }
}