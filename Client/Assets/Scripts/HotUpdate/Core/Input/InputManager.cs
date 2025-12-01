using System.Collections.Generic;

namespace GameFramework.Core 
{
    public interface IInputable
    {
        void InputAction(InputContext context);
    }

    public struct InputEventArgs : IGameEventArgs 
    {
        public InputContext context;
    }

    [GameModule]
    public class InputManager : IGameModule_SyncInit, IFixedUpdate
    {
        private GameInput m_GameInput;
        private InputActionWrapper[] m_Wrappers;

        public void Init()
        {
            m_GameInput = new GameInput();
            m_GameInput.Enable();
            InitWrappers();
        }

        public void FixedUpdate()
        {
            foreach (var item in m_Wrappers)
            {
                item.Tick();
            }
        }

        public void Enable() 
        {
            m_GameInput?.Enable();
        }

        public void Disable() 
        {
            m_GameInput?.Disable();
        }

        private void InitWrappers() 
        {
            List<InputActionWrapper> wrappers = new List<InputActionWrapper>();

            InputActionWrapper wrapper0 = new GMActionWrapper();
            wrapper0.Initialize(m_GameInput.Player.GM);
            wrapper0.OnInput += OnInputHandler;
            wrappers.Add(wrapper0);

            InputActionWrapper wrapper1 = new SubmitActionWrapper();
            wrapper1.Initialize(m_GameInput.Player.Submit);
            wrapper1.OnInput += OnInputHandler;
            wrappers.Add(wrapper1);

            InputActionWrapper wrapper2 = new CancelActionWrapper();
            wrapper2.Initialize(m_GameInput.Player.Cancel);
            wrapper2.OnInput += OnInputHandler;
            wrappers.Add(wrapper2);

            InputActionWrapper wrapper3 = new EscActionWrapper();
            wrapper3.Initialize(m_GameInput.Player.ESC);
            wrapper3.OnInput += OnInputHandler;
            wrappers.Add(wrapper3);

            InputActionWrapper wrapper4 = new LeftShiftActionWrapper();
            wrapper4.Initialize(m_GameInput.Player.LeftShift);
            wrapper4.OnInput += OnInputHandler;
            wrappers.Add(wrapper4);

            InputActionWrapper wrapper5 = new MoveActionWrapper();
            wrapper5.Initialize(m_GameInput.Player.Move);
            wrapper5.OnInput += OnInputHandler;
            wrappers.Add(wrapper5);

            InputActionWrapper wrapper6 = new Move2ActopmWrapper();
            wrapper6.Initialize(m_GameInput.Player.Move2);
            wrapper6.OnInput += OnInputHandler;
            wrappers.Add(wrapper6);

            InputActionWrapper wrapper7 = new M_KeyboardActionWrapper();
            wrapper7.Initialize(m_GameInput.Player.M_Keyboard);
            wrapper7.OnInput += OnInputHandler;
            wrappers.Add(wrapper7);

            InputActionWrapper wrapper8 = new PageActionWrapper();
            wrapper8.Initialize(m_GameInput.Player.Page);
            wrapper8.OnInput += OnInputHandler;
            wrappers.Add(wrapper8);

            m_Wrappers = wrappers.ToArray();
        }

        private void OnInputHandler(InputContext context)
        {
            //MDebug.Log("OnInputHandler", context);
            Game.Event.Publish(new InputEventArgs() { context = context });
        }
    }
}