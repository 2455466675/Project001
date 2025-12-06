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
            PopAllCammand();
            current = inputModules.Find(a => a.ModuleType == moduleType);
        }

        public void InputAction(InputContext context) 
        {
            current?.InputAction(context);
        }

        public void PopAllCammand() 
        {
            current?.PopAll();
        }

        public void PushCammand(InputCammand cammand)
        {
            current?.Push(cammand);
        }

        public void PopCammand()
        {
            current?.Pop();
        }

        public bool TryPeek<T>(out T cmd) where T : InputCammand
        {
            if (current == null)
            {
                cmd = default;
                return false;
            }

            if (current.TryPeek(out cmd))
            {
                return true;
            }
            else
            {
                return false;                
            }
        }
    }
}