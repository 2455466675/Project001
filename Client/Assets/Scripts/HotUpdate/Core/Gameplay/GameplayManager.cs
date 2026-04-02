using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    internal class GameplayManager
    {
        private class Gameplay
        {
            public int priority;
            public IGameplay inst;
        }

        private Dictionary<Type, Gameplay> gameplays;
        private List<IGameplay> sortGameplays;
        private List<IUpdateable> updateableModules;
        private List<IFixedUpdateable> fixedUpdateableModules;
        private List<ILateUpdateable> lateUpdateableModules;

        public void Init()
        {
            InitGameplays();
            RegisterSavableGameplay();

            foreach (var gameplay in sortGameplays)
            {
                gameplay.OnInit();
            }
        }

        public void Exit()
        {
            foreach (var gameplay in sortGameplays)
            {
                gameplay.OnExit();
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < updateableModules.Count; i++)
            {
                updateableModules[i].Update(deltaTime);
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            for (int i = 0; i < fixedUpdateableModules.Count; i++)
            {
                fixedUpdateableModules[i].FixedUpdate(fixedDeltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (int i = 0; i < lateUpdateableModules.Count; i++)
            {
                lateUpdateableModules[i].LateUpdate(deltaTime);
            }
        }

        public T GetModule<T>() where T : class, IGameplay
        {
            Type type = typeof(T);
            if (gameplays.ContainsKey(type))
            {
                return gameplays[type].inst as T;
            }
            else
            {
                return default;
            }
        }

        private void InitGameplays()
        {
            gameplays = new Dictionary<Type, Gameplay>();
            sortGameplays = new List<IGameplay>();
            updateableModules = new List<IUpdateable>();
            fixedUpdateableModules = new List<IFixedUpdateable>();
            lateUpdateableModules = new List<ILateUpdateable>();

            List<Gameplay> sortList = new List<Gameplay>();
            var items = Game.GetTypes<GameplayAttribute>();
            foreach (var item in items)
            {
                Type type = item.type;
                if (gameplays.ContainsKey(type))
                {
                    continue;
                }

                var obj = Activator.CreateInstance(type);
                if (obj is IGameplay inst)
                {
                    Gameplay gameplay = new Gameplay()
                    {
                        inst = inst,
                        priority = item.priority,
                    };

                    gameplays.Add(type, gameplay);
                    sortList.Add(gameplay);
                }
                else
                {
                    Debug.LogError("Type is not IGameplay : " + type.FullName);
                }
            }

            sortList.Sort((a, b) => a.priority - b.priority);
            foreach (var item in sortList)
            {
                IGameplay inst = item.inst;
                if (inst is IUpdateable u)
                {
                    updateableModules.Add(u);
                }

                if (inst is IFixedUpdateable fu)
                {
                    fixedUpdateableModules.Add(fu);
                }

                if (inst is ILateUpdateable lu)
                {
                    lateUpdateableModules.Add(lu);
                }

                sortGameplays.Add(inst);
            }
        }

        private void RegisterSavableGameplay()
        {
            GameSaveSystem saveSystem = Game.GetSystem<GameSaveSystem>();
            foreach (var gameplay in sortGameplays)
            {
                if (gameplay is IGameSavable savable)
                {
                    saveSystem.Register(savable);
                }
            }
        }
    }
}
