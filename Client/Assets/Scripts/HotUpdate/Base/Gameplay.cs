using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework 
{
    public class GameplayAttribute : GameAttribute
    {
        public int Piority { get; private set; }
        public GameplayAttribute(int priority = 0) 
        { 
            this.Piority = priority;
        }
    }

    internal class Gameplay : IGameplay
    {
        private class GameplaySystemSort 
        {
            public int priority;
            public IGameplaySystem system;
        }

        private Dictionary<Type, IGameplaySystem> m_Systems;
        private List<GameplaySystemSort> m_Sorts;
   
        internal Gameplay() { }

        public void Init()
        {
            Type[] types = Game.GetTypes<GameplayAttribute>();
            m_Systems = new Dictionary<Type, IGameplaySystem>(types.Length);
            m_Sorts = new List<GameplaySystemSort>(types.Length);

            foreach (Type type in types)
            {
                object o = Activator.CreateInstance(type);
                if (o is IGameplaySystem s)
                {
                    m_Systems[type] = s;

                    var attribute = type.GetCustomAttribute(typeof(GameplayAttribute), false) as GameplayAttribute;
                    var sort = new GameplaySystemSort();
                    sort.priority = attribute.Piority;
                    sort.system = s;
                    m_Sorts.Add(sort);
                }
            }

            m_Sorts.Sort((a, b) => a.priority - b.priority);
            foreach (var item in m_Sorts)
            {
                item.system.OnInit();
            }
        }

        public void Exit()
        {
            foreach (var item in m_Sorts)
            {
                item.system.OnExit();
            }
        }

        public void SaveGame(ISaveWriter writer) 
        {
            for (int i = 0; i < m_Sorts.Count; i++)
            {
                var item = m_Sorts[i];
                var sys = item.system;
                writer.Next(sys.GetType().Name);
                sys.OnSaveGame(writer);
            }
        }

        public void LoadGame(ISaveReader reader) 
        {
            for (int i = 0; i < m_Sorts.Count; i++)
            {
                var item = m_Sorts[i];
                var sys = item.system;
                reader.Next(sys.GetType().Name);
                sys.OnLoadGame(reader);
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
    }
}