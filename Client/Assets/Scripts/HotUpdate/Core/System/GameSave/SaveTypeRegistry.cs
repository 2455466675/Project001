using System;
using System.Collections.Generic;

namespace GameFramework.Core
{
    /// <summary>
    /// 存档类型注册表。用稳定的整型 id 标识可存档类型,序列化时只记录 id 而非类名,
    /// 这样后续重命名/移动类不会破坏旧存档的兼容性,反序列化也无需反射类名。
    /// </summary>
    public static class SaveTypeRegistry
    {
        private static readonly Dictionary<int, Func<ISavableData>> IdToFactory = new Dictionary<int, Func<ISavableData>>();
        private static readonly Dictionary<Type, int> TypeToId = new Dictionary<Type, int>();

        public static void Register<T>(int typeId) where T : ISavableData, new()
        {
            IdToFactory[typeId] = () => new T();
            TypeToId[typeof(T)] = typeId;
        }

        public static int GetTypeId(ISavableData data)
        {
            if (TypeToId.TryGetValue(data.GetType(), out int id))
            {
                return id;
            }
            throw new Exception($"未注册的存档类型: {data.GetType().FullName}");
        }

        public static ISavableData Create(int typeId)
        {
            if (IdToFactory.TryGetValue(typeId, out var factory))
            {
                return factory();
            }
            throw new Exception($"未知的存档类型 id: {typeId}");
        }
    }
}
