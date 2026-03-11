using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public class GameplayManager
    {
        private class Gameplay
        {
            public int priority;
            public IGameplay inst;
        }

        private Dictionary<Type, Gameplay> gameplays;
        private List<IGameplay> sortGameplays;

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

        private void InitGameplays()
        {
            gameplays = new Dictionary<Type, Gameplay>();
            sortGameplays = new List<IGameplay>();

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
                sortGameplays.Add(item.inst);
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
