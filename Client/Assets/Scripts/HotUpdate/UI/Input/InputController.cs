using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.UI 
{
    [GameModule]
    public class InputController : IGameModule_SyncInit
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

        public void EnterNavigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs) 
        {
            current?.EnterNavigate(navigationDefine, panelDefine, defaultIndexs);
        }

        public void ExitNavigate() 
        {
            current?.ExitNavigate();
        }

        public void PushCammand(GameCammand cammand)
        {
            current?.PushCammand(cammand);
        }
    }
}