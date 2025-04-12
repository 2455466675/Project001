using System;
using System.Collections.Generic;

namespace Game.System
{
    public class UnitArchType 
    {
        public List<Type> types;

        public void F() 
        {
            foreach (var type in types) 
            {
                if (type.IsSubclassOf(typeof(UnitComponent))) 
                {
                    
                }

                Activator.CreateInstance(type);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem
    {
        public ActorContainer Container { get; private set; }

        private Character character;

        private List<Character> characters;

        private HashSet<IFixedUpdateComponent> fixedUpdateComponents;
        private List<IFixedUpdateComponent> newfixedUpdateComponents;
        private List<IFixedUpdateComponent> oldfixedUpdateComponents;

        public void Init(GameInitConfig config)
        {
            fixedUpdateComponents = new HashSet<IFixedUpdateComponent>();
            newfixedUpdateComponents = new List<IFixedUpdateComponent>();
            oldfixedUpdateComponents = new List<IFixedUpdateComponent>();

            var go = Game.Resource.LoadAndInstantiate(config.ActorContainerPath, Game.Root.transform);
            Container = go.GetComponent<ActorContainer>();

            characters = new List<Character>();

            Character character = new Character();
            character.Init(810001);
            this.character = character;

            characters.Add(character);

            for (int i = 2; i < 5; i++) 
            {
                Character c = new Character();
                c.Init(810000 + i);
                characters.Add(c);
            }
        }

        public void Move(float x, float y) 
        {
            character.Move(x, y);
        }

        public void Run(bool isRunning)
        {
            character.Run(isRunning);
        }

        public void FixedUpdate(float fdt) 
        {
            if (newfixedUpdateComponents.Count > 0)
            {
                for (int i = 0; i < newfixedUpdateComponents.Count; i++)
                {
                    fixedUpdateComponents.Add(newfixedUpdateComponents[i]);
                }
                newfixedUpdateComponents.Clear();
            }

            if (oldfixedUpdateComponents.Count > 0)
            {
                for (int i = 0; i < oldfixedUpdateComponents.Count; i++)
                {
                    fixedUpdateComponents.Remove(oldfixedUpdateComponents[i]);
                }
                oldfixedUpdateComponents.Clear();
            }

            foreach (var component in fixedUpdateComponents)
            {
                component.FixedUpdate(fdt);
            }
        }

        public RoleUnit CreateUnit()
        {
            return default;
        }

        public T CreateComponent<T>() where T : UnitComponent, new()
        {
            T component = new();
            if (component is IAwakeComponent a) 
            {
                a.Awake();
            }
            if (component is IFixedUpdateComponent u) 
            {
                newfixedUpdateComponents.Add(u);
            }
            return component;
        }

        public void DestroyComponent(UnitComponent component) 
        {
            if (component == null)
            {
                return;
            }

            if (component is not IFixedUpdateComponent u) 
            {
                return;
            }
            oldfixedUpdateComponents.Add(u);
        }
    }
}
