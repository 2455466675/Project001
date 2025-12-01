using System;
using UnityEngine;

//[Serializable]
//public abstract class TestA
//{
//    public string name;
//    public abstract void Fun();
//}

//public class A : TestA
//{
//    public override void Fun()
//    {
//    }
//}

//[AbstractType(typeof(TestA))]
//[SerializeReference]
//public TestA[] testA;

/// <summary>
/// 抽象类型序列化
/// 需要配合[SerializeReference]特性使用
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class AbstractTypeAttribute : PropertyAttribute
{
    public Type type;

    public AbstractTypeAttribute(Type type)
    {
        this.type = type; 
    }
}
