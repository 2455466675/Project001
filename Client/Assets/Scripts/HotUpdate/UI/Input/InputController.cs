using Cysharp.Threading.Tasks;
using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.UI 
{
    public class NavigateCammand : InputCammand 
    {
        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            PanelCammand panelCammand;

            if (TryPeek(out PanelCammand cammand) && cammand.Define == panelDefine)
            {
                panelCammand = cammand;
            }
            else
            {
                panelCammand = new PanelCammand(panelDefine);
                Push(panelCammand);
            }

            if (panelCammand.TryPeek(out NavigationListCammand subCammand) && subCammand.Define == navigationDefine) 
            {
                return;
            }

            NavigationListCammand listCammand = new NavigationListCammand(navigationDefine, defaultIndexs);
            panelCammand.Push(listCammand);
        }

        protected override void OnInputAction(InputContext context)
        {
            InputDefine inputDefine = context.Input;
            switch (inputDefine)
            {
                case InputDefine.Cancel:
                    Pop();
                    break;
                case InputDefine.Esc:
                    PopAll();
                    break;
            }
        }
    }

    public enum InputModuleType 
    {
        Character = 1,
        Battle = 2,
    }

    public abstract class InputModule : IInputable
    {
        private readonly NavigateCammand navigateCammand;

        public abstract InputModuleType ModuleType { get; }

        public InputModule() 
        {
            navigateCammand = new NavigateCammand();
        }

        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            navigateCammand.Navigate(navigationDefine, panelDefine, defaultIndexs);
        }

        public void InputAction(InputContext context)
        {
            if (navigateCammand.Count > 0) 
            {
                navigateCammand.InputAction(context);
            }
            else
            {
                OnInputAction(context);
            }
        }

        protected abstract void OnInputAction(InputContext context);
    }

    public class CharacterInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Character;

        protected override void OnInputAction(InputContext context)
        {
            
        }
    }

    public class BattleInputModule : InputModule
    {
        public override InputModuleType ModuleType => InputModuleType.Battle;

        protected override void OnInputAction(InputContext context)
        {

        }
    }

    [GameModule(GameModulePriority.InputController)]
    public class InputController : ISyncInit
    {
        private List<InputModule> inputModules;
        private InputModule current;

        public void Init()
        {
            inputModules = new List<InputModule>
            {
                new CharacterInputModule(),
                new BattleInputModule()
            };
        }

        public void Switch(InputModuleType moduleType) 
        {
            current = inputModules.Find(a => a.ModuleType == moduleType);
        }

        public void InputAction(InputContext context) 
        {
            current?.InputAction(context);
        }

        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs) 
        {
            current?.Navigate(navigationDefine, panelDefine, defaultIndexs);
        }
    }
}