using Game.Input;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class InputController : InputCammand
    {
        private readonly List<InputModule> modules;

        public InputController()
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
                        if (cammand.Count == 0 && !cammand.IsStatic) 
                        {
                            Pop();
                        }
                    }
                    break;
            }
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Panel)
        {
            InputModule module;

            if (TryPeek(out InputCammand cammand))
            {
                module = cammand as InputModule;              
                if (module != null && module.ModuleType == moduleType) 
                {
                    module.Navigate(list_ID);
                    return;
                }
            }

            module = modules.Find(m => m.ModuleType == moduleType);
            if (module == null)
            {
                MLog.Error($"InputModule is null : {moduleType}");
                return;
            }

            Push(module);
            module.Navigate(list_ID);
        }
    }
}
