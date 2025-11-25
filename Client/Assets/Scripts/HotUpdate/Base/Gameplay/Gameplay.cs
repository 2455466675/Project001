using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GameFramework 
{
    /// <summary>
    /// 游戏玩法特性类
    /// 通过Tools/MyTools/Game Priority Editor来编辑优先级
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameplayAttribute : GameAttribute
    {
    }

    internal class Gameplay : IGameplay
    {
        private class GameplaySystemSort 
        {
            public int priority;
            public IGameplaySystem system;
        }

        private class GameplaySerializeableSort
        {
            public int priority;
            public IGameSerializeable serializeable;
        }

        private Dictionary<Type, IGameplaySystem> m_Systems;
        private List<GameplaySystemSort> m_SystemSort;
        private List<GameplaySerializeableSort> m_SerializeableSort;

        internal Gameplay() { }

        public async UniTask Init()
        {
            List<ClassPriorityData> items = await Game.GetGamePriorityDatas("GameModuleAttribute");
            int GetPriority(string fullName)
            {
                foreach (var data in items)
                {
                    if (data.type == fullName)
                    {
                        return data.priority;
                    }
                }
                return 0;
            }

            Type[] types = Game.GetTypes<GameplayAttribute>();
            m_Systems = new Dictionary<Type, IGameplaySystem>(types.Length);
            m_SystemSort = new List<GameplaySystemSort>();
            m_SerializeableSort = new List<GameplaySerializeableSort>();

            foreach (Type type in types)
            {
                int priority = GetPriority(type.FullName);
                object o = Activator.CreateInstance(type);
                if (o is IGameplaySystem sys)
                {
                    m_Systems[type] = sys;
                    m_SystemSort.Add(new GameplaySystemSort(){ priority = priority, system = sys });
                }
                if (o is IGameSerializeable serializeable)
                {
                    m_SerializeableSort.Add(new GameplaySerializeableSort(){ priority = priority, serializeable = serializeable });
                }
            }

            m_SystemSort.Sort((a, b) => a.priority - b.priority);
            m_SerializeableSort.Sort((a, b) => a.priority - b.priority);

            InitSystem();
        }

        public void Exit()
        {
            ExitSystem();
        }

        public void SaveGame(ISaveWriter writer) 
        {
            for (int i = 0; i < m_SerializeableSort.Count; i++)
            {
                var item = m_SerializeableSort[i];
                var serializeable = item.serializeable;
                writer.Next(serializeable.GetType().Name);
                serializeable.OnSaveGame(writer);
            }
        }

        public void LoadGame(ISaveReader reader) 
        {
            for (int i = 0; i < m_SerializeableSort.Count; i++)
            {
                var item = m_SerializeableSort[i];
                var serializeable = item.serializeable;
                reader.Next(serializeable.GetType().Name);
                serializeable.OnLoadGame(reader);
            }
        }

        public T GetSystem<T>() where T : class, IGameplaySystem 
        {
            Type type = typeof(T);
            if (m_Systems.ContainsKey(type)) 
            {
                return m_Systems[type] as T;
            }

            return default;
        }

        private void InitSystem()
        {
            foreach (var item in m_SystemSort)
            {
                item.system.OnInit();
            }
        }

        private void ExitSystem()
        {
            foreach (var item in m_SystemSort)
            {
                item.system.OnExit();
            }
        }
    }
}