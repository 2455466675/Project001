using System;
using System.Reflection;
using System.Text;

namespace Game.Cfg

{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CfgDataBase
    {
        public readonly int id;
        public override string ToString()
        {
            StringBuilder sb = new();
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo property in fields)
            {
                sb.AppendLine($"{property.Name}: {property.GetValue(this)}");
            }
            return sb.ToString();
        }
    }
}