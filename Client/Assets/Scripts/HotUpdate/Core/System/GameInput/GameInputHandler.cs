using System;
using System.Collections.Generic;

namespace GameFramework.Core
{
    internal class GameInputHandler
    {
        private Stack<IInputable> priorInputHandler;
        private List<InputModuleBase> basicInputModules;
        private InputModuleBase currentModule;

        public void Init()
        {
            priorInputHandler = new Stack<IInputable>();

            basicInputModules = new List<InputModuleBase>();
            var items = Game.GetTypes<GameInputModuleAttribute>();
            foreach (var item in items)
            {
                Type type = item.type;
                var obj = Activator.CreateInstance(type);
                basicInputModules.Add(obj as InputModuleBase);
            }
        }

        public void PushInputHandler(IInputable handler)
        {
            priorInputHandler.Push(handler);
        }

        public void PopInputHandler()
        {
            priorInputHandler.Pop();
        }

        public void Switch(InputModuleType moduleType)
        {
            if (currentModule != null && currentModule.ModuleType == moduleType)
            {
                return;
            }
            currentModule = basicInputModules.Find(m => m.ModuleType == moduleType);
        }

        public void OnInput(InputContext context)
        {
            if (priorInputHandler.Count > 0)
            {
                priorInputHandler.Peek().OnInput(context);
            }
            else
            {
                currentModule?.OnInput(context);                
            }
        }
    }
}

