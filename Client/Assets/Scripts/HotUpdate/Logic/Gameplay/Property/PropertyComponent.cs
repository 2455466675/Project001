using ECS;
using GameFramework.Utility.GameDefine;
using System;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IPropertyReader
    {
        int GetPropertyValue(PropertyDefine pType);
    }

    public interface IPropertyWriter
    {
        void AddModifier(PropertyModifier m);
    }

    public interface IPropertyComponent : IPropertyReader, IPropertyWriter
    {

    }

    public abstract class Property
    {
        private class Attribute : Property
        {
            public override int Value
            {
                get
                {
                    if (isDirty)
                    {
                        currValue = BaseValue; //最终值 = ((基础值 + ΣAdd) × (1 + ΣPercentAdd)) × Π(Multiply)
                        isDirty = false;
                    }
                    return currValue;
                }
            }

            private int currValue;
            private bool isDirty;
            private readonly List<PropertyModifier> modifiers;

            public Attribute(PropertyDefine pType, int baseValue) : base(pType, baseValue)
            {
                modifiers = new List<PropertyModifier>();
                isDirty = true;
            }

            public override void AddModifier(PropertyModifier m)
            {
                modifiers.Add(m);
                isDirty = true;
            }

            public override void RemoveModifier(PropertyModifier m)
            {
                modifiers.Remove(m);
                isDirty = true;
            }
        }
        private class Resource : Property
        {
            public override int Value => currValue;
            private int currValue;

            public Resource(PropertyDefine pType, int baseValue) : base(pType, baseValue)
            {
                currValue = baseValue;
            }

            public override void AddModifier(PropertyModifier m)
            {
                if (m.Op != ModifierOp.Add)
                {
                    throw new NotSupportedException($"{PType} : 资源型属性只能进行加减值的操作！");
                }
                currValue = Utility.GameMath.Max(0, currValue + m.Value);
            }

            public override void RemoveModifier(PropertyModifier m)
            {
                throw new NotSupportedException($"{PType} : 资源型属性不支持此操作！");
            }
        }

        public static Property CreateProperty(PropertyDefine pType, int baseValue)
        {
            if (pType == PropertyDefine.CurHp)
            {
                return new Resource(pType, baseValue);
            }
            else
            {
                return new Attribute(pType, baseValue);
            }
        }

        public PropertyDefine PType { get; private set; }
        public int BaseValue { get; private set; }
        public abstract int Value { get; }

        protected Property(PropertyDefine pType, int baseValue)
        {
            this.PType = pType;
            this.BaseValue = baseValue;
        }

        public abstract void AddModifier(PropertyModifier m);
        public abstract void RemoveModifier(PropertyModifier m);
    }

    public enum ModifierOp
    {
        Add        = 100000,   // 加固定值:+100 攻击
        PercentAdd = 200000,   // 加百分比(同通道相加):+50% 与 +30% => +80%
        Multiply   = 300000,   // 独立乘区(连乘):易伤 ×1.2
    }

    public class PropertyModifier
    {
        public PropertyDefine Target { get; set; }
        public ModifierOp Op { get; set; }
        public int Value { get; set; }
        public object Source { get; set; }
    }

    public class PropertyComponent : ComponentBase, IPropertyComponent
    {
        private Dictionary<PropertyDefine, Property> propertyMap;

        protected override void Awake()
        {
            propertyMap = new Dictionary<PropertyDefine, Property>();
        }

        public void CreateProperty(PropertyDefine propertyType, int baseValue)
        {
            if (propertyMap.ContainsKey(propertyType))
            {
                return;
            }
            var property = Property.CreateProperty(propertyType, baseValue);
            propertyMap[propertyType] = property;
        }

        public void AddModifier(PropertyModifier m)
        {
            if (!propertyMap.TryGetValue(m.Target, out var property))
            {
                property = Property.CreateProperty(m.Target, 0);
                propertyMap[m.Target] = property;
            }
            property.AddModifier(m);            
        }

        public int GetPropertyValue(PropertyDefine pType)
        {
            if (propertyMap.TryGetValue(pType, out var property))
            {
                return property.Value;
            }
            else
            {
                return 0;
            }
        }
    }
}
