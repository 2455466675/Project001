using Game.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Input
{
    public class InputModuleManager : InputCammand
    {
        private readonly List<InputModule> modules;

        public InputModuleManager() 
        {
            modules = new List<InputModule>
            {
                new BasalInputModule(),
                new PanelInputModule(),
                new BattleInputModule()
            };

            Push(modules.Find(m => m.ModuleType == ModuleType.Basal));
        }

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Cancel:
                case InputType.Esc:
                    if (TryPeek(out InputCammand cammand))
                    {
                        if (cammand.Count == 0 && !cammand.IsLocked)
                        {
                            Pop();
                        }
                    }
                    break;
            }
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType, int[] navigateIndexs, object intent)
        {
            InputModule module;

            if (TryPeek(out InputCammand cammand))
            {
                module = cammand as InputModule;
                if (module != null)
                {
                    if (moduleType == ModuleType.Undefined || module.ModuleType == moduleType)
                    {
                        module.Navigate(list_ID, navigateIndexs, intent);
                        return;
                    }
                }
            }

            module = modules.Find(m => m.ModuleType == moduleType);
            if (module == null)
            {
                MLog.Error($"InputModule is null : {moduleType}");
                return;
            }

            Push(module);
            module.Navigate(list_ID, navigateIndexs, intent);
        }
    }
}