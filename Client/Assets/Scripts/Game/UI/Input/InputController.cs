using System;

namespace Game.UI.Input
{
    public enum InputType
    {
        Move      = 0,
        Submit    = 1,
        Cancel    = 2,
        Esc       = 3,
        LeftShift = 4,
        Map       = 5,
        GM        = 6,
    }

    /// <summary>
    /// 
    /// </summary>
    public class InputController
    {
        private readonly GameInput gameInput;
        private readonly InputModuleManager moduleManager;

        private InputActionWrapper[] wrappers;

        public InputController()
        {
            gameInput = new GameInput();
            gameInput.Enable();

            wrappers = new InputActionWrapper[Enum.GetValues(typeof(InputType)).Length];

            var move = new MoveActionWrapper();
            move.Initialize(gameInput.DefaultMap.Move);
            move.ActionEvent += OnMove;
            wrappers[0] = move;

            var submit = new SubmitActionWrapper();
            submit.Initialize(gameInput.DefaultMap.Submit);
            submit.ActionEvent += OnSubmit;
            wrappers[1] = submit;

            var cancel = new CancelActionWrapper();
            cancel.Initialize(gameInput.DefaultMap.Cancel);
            cancel.ActionEvent += OnCancel;
            wrappers[2] = cancel;

            var esc = new EscActionWrapper();
            esc.Initialize(gameInput.DefaultMap.Esc);
            esc.ActionEvent += OnEsc;
            wrappers[3] = esc;

            var ls = new LeftShiftActionWrapper();
            ls.Initialize(gameInput.DefaultMap.LeftShift);
            ls.ActionEvent += OnLeftShift;
            wrappers[4] = ls;

            var map = new MapActionWrapper();
            map.Initialize(gameInput.DefaultMap.Map);
            map.ActionEvent += OnMap;
            wrappers[5] = map;

            var gm = new GMActionWrapper();
            gm.Initialize(gameInput.DefaultMap.GM);
            gm.ActionEvent += OnGM;
            wrappers[6] = gm;

            moduleManager = new InputModuleManager();
        }

        public void FixedUpdate(float dt)
        {
            for (int i = 0; i < wrappers.Length; i++) 
            {
                wrappers[i]?.Tick(dt);
            }
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType, int[] navigateIndexs, object intent)
        {
            moduleManager.Navigate(list_ID, moduleType, navigateIndexs, intent);            
        }

        public void PushModule(ModuleType moduleType) 
        {
            moduleManager.PushModule(moduleType);
        }

        public void Back() 
        {
            OnCancel();
        }

        public void Close() 
        {
            OnEsc();
        }

        private void OnMove(float x, float y)
        {
            ActionContext context = new()
            {
                InputType = InputType.Move,
                Vector2Value = new UnityEngine.Vector2(x, y)
            };
            DoAction(context);
        }
        private void OnSubmit()
        {
            ActionContext context = new()
            {
                InputType = InputType.Submit,
            };
            DoAction(context);
        }
        private void OnCancel()
        {
            ActionContext context = new()
            {
                InputType = InputType.Cancel,
            };
            DoAction(context);
        }
        private void OnEsc()
        {
            ActionContext context = new()
            {
                InputType = InputType.Esc,
            };
            DoAction(context);
        }
        private void OnLeftShift(bool isPress)
        {
            ActionContext context = new()
            {
                InputType = InputType.LeftShift,
                BoolValue = isPress
            };
            DoAction(context);
        }
        private void OnMap()
        {
            ActionContext context = new()
            {
                InputType = InputType.Map,
            };
            DoAction(context);
        }

        private void OnGM() 
        {
            ActionContext context = new()
            {
                InputType = InputType.GM,
            };
            DoAction(context);
        }

        private void DoAction(ActionContext context)
        {
            moduleManager.InputAction(context);
        }
    }
}
