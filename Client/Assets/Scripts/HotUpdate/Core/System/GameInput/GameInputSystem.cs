using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameInputSystem : IGameSystem, IInit, IFixedUpdateable
    {
        private GameInput gameInput;
        private InputActionWrapper[] wrappers;

        private GameInputHandler inputHandler;

        void IInit.Init()
        {
            gameInput = new GameInput();
            gameInput.Enable();

            InitWrappers();

            inputHandler = new GameInputHandler();
            inputHandler.Init();
        }

        void IFixedUpdateable.FixedUpdate(float fdt)
        {
            foreach (var item in wrappers)
            {
                item.Tick();
            }
        }

        public void Enable()
        {
            gameInput?.Enable();
        }

        public void Disable()
        {
            gameInput?.Disable();
        }

        public void PushInputHandler(IInputable handler)
        {
            inputHandler.PushInputHandler(handler);
        }

        public void PopInputHandler()
        {
            inputHandler.PopInputHandler();
        }

        public void Switch(InputModuleType moduleType)
        {
            inputHandler.Switch(moduleType);
        }

        private void InitWrappers()
        {
            List<InputActionWrapper> wrappers = new List<InputActionWrapper>();

            InputActionWrapper wrapper0 = new GMActionWrapper();
            wrapper0.Initialize(gameInput.Player.GM);
            wrapper0.OnInput += OnInput;
            wrappers.Add(wrapper0);

            InputActionWrapper wrapper1 = new SubmitActionWrapper();
            wrapper1.Initialize(gameInput.Player.Submit);
            wrapper1.OnInput += OnInput;
            wrappers.Add(wrapper1);

            InputActionWrapper wrapper2 = new CancelActionWrapper();
            wrapper2.Initialize(gameInput.Player.Cancel);
            wrapper2.OnInput += OnInput;
            wrappers.Add(wrapper2);

            InputActionWrapper wrapper3 = new EscActionWrapper();
            wrapper3.Initialize(gameInput.Player.ESC);
            wrapper3.OnInput += OnInput;
            wrappers.Add(wrapper3);

            InputActionWrapper wrapper4 = new LeftShiftActionWrapper();
            wrapper4.Initialize(gameInput.Player.LeftShift);
            wrapper4.OnInput += OnInput;
            wrappers.Add(wrapper4);

            InputActionWrapper wrapper5 = new MoveActionWrapper();
            wrapper5.Initialize(gameInput.Player.Move);
            wrapper5.OnInput += OnInput;
            wrappers.Add(wrapper5);

            Move2ActionWrapper wrapper6 = new Move2ActionWrapper();
            wrapper6.Initialize(gameInput.Player.Move2);
            wrapper6.OnInput += OnInput;
            wrappers.Add(wrapper6);

            InputActionWrapper wrapper7 = new M_KeyboardActionWrapper();
            wrapper7.Initialize(gameInput.Player.M_Keyboard);
            wrapper7.OnInput += OnInput;
            wrappers.Add(wrapper7);

            this.wrappers = wrappers.ToArray();
        }

        private void OnInput(InputContext context)
        {
            //MDebug.Log("OnInput", context);
            inputHandler.OnInput(context);
        }
    }
}


