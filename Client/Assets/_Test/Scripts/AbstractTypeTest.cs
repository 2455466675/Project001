using System;
using UnityEngine;

[Serializable]
public abstract class Base
{
    public string name;

    public abstract void Fun();
}

public class TestA : Base
{
    public int a;
    public override void Fun()
    {
        Debug.Log("TestA = " + a);
    }
}

public class TestB : Base
{
    public float b;
    public override void Fun()
    {
        Debug.Log("TestB = " + b);
    }
}

public class TestC : Base
{
    public bool c;
    public override void Fun()
    {
        Debug.Log("TestC = " + c);
    }
}

public class AbstractTypeTest : MonoBehaviour
{
    [AbstractType(typeof(Base))]
    [SerializeReference]
    public Base item;

    [AbstractType(typeof(Base))]
    [SerializeReference]
    public Base[] items;
}
