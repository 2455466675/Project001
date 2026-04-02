using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    internal interface IGameSystemManager
    {
        T GetSystem<T>() where T : class, IGameSystem;
    }

    internal class GameSystemManager : IGameSystemManager, IUpdateable, IFixedUpdateable
    {
        private class GameSystem
        {
            public int priority;
            public IGameSystem inst;
        }

        private Dictionary<Type, GameSystem> systems;
        private List<IUpdateable> updateableSystems;
        private List<IFixedUpdateable> fixedUpdateableSystems;
        private List<ILateUpdateable> lateUpdateableSystems;

        public async UniTask Init()
        {
            systems = new Dictionary<Type, GameSystem>();
            updateableSystems = new List<IUpdateable>();
            fixedUpdateableSystems = new List<IFixedUpdateable>();
            lateUpdateableSystems = new List<ILateUpdateable>();

            List<GameSystem> sortList = new List<GameSystem>();
            var items = Game.GetTypes<GameSystemAttribute>();
            foreach (var item in items)
            {
                Type type = item.type;
                if (systems.ContainsKey(type))
                {
                    continue;
                }

                var obj = Activator.CreateInstance(type);
                if (obj is IGameSystem inst)
                {
                    GameSystem system = new GameSystem()
                    {
                        inst = inst,
                        priority = item.priority,
                    };

                    systems.Add(type, system);
                    sortList.Add(system);
                }
                else
                {
                    Debug.LogError("Type is not IGameSystem : " + type.FullName);
                }

            }

            sortList.Sort((a, b) => a.priority - b.priority);

            foreach (var system in sortList)
            {
                IGameSystem inst = system.inst;
                if (inst is IUpdateable u)
                {
                    updateableSystems.Add(u);
                }

                if (inst is IFixedUpdateable fu)
                {
                    fixedUpdateableSystems.Add(fu);
                }

                if (inst is ILateUpdateable lu)
                {
                    lateUpdateableSystems.Add(lu);
                }

                if (inst is IInit i)
                {
                    i.Init();
                }

                if (inst is IAsyncInit ai)
                {
                    await ai.Init();
                }

                Debug.Log("GameSystem Init : " + system.inst.GetType().FullName);
            }
        }

        public T GetSystem<T>() where T : class, IGameSystem
        {
            Type type = typeof(T);
            if (systems.TryGetValue(type, out GameSystem system))
            {
                return system.inst as T;
            }
            else
            {
                return default;
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < updateableSystems.Count; i++)
            {
                updateableSystems[i].Update(deltaTime);
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            for (int i = 0; i < fixedUpdateableSystems.Count; i++)
            {
                fixedUpdateableSystems[i].FixedUpdate(fixedDeltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (int i = 0; i < lateUpdateableSystems.Count; i++)
            {
                lateUpdateableSystems[i].LateUpdate(deltaTime);
            }
        }
    }
}