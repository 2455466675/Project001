using System.Collections.Generic;

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

            PushModule(ModuleType.Basal);
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
            InputModule module = PushModule(moduleType);
            if (module == null) 
            {
                return;
            }
            module.Navigate(list_ID, navigateIndexs, intent);
        }

        public InputModule PushModule(ModuleType moduleType) 
        {
            InputModule module;

            if (TryPeek(out InputCammand cammand))
            {
                module = cammand as InputModule;
                if (module != null && (moduleType == ModuleType.Undefined || module.ModuleType == moduleType))
                {                    
                    return module;
                }
            }

            module = modules.Find(m => m.ModuleType == moduleType);
            if (module == null)
            {
                MLog.Error($"InputModule is null : {moduleType}");
                return null;
            }

            Push(module);
            return module;
        }
    }
}