using System;
using Cysharp.Threading.Tasks;
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

    public class InputManager : IGameModule
    {
        public GameModulePriority Priority => GameModulePriority.InputManager;

        private GameInput m_GameInput;
        private InputActionWrapper[] m_Wrappers;

        public async UniTask Init()
        {
            m_GameInput = new GameInput();
            m_GameInput.Enable();
            InitWrappers();

            await UniTask.Yield();
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

            InputActionWrapper wrapper6 = new M_KeyboardActionWrapper();
            wrapper6.Initialize(m_GameInput.Player.M_Keyboard);
            wrapper6.OnInput += OnInputHandler;
            wrappers.Add(wrapper6);

            m_Wrappers = wrappers.ToArray();
        }

        private void OnInputHandler(InputContext context)
        {
            MDebug.Log("OnInputHandler", context);
            Game.GetModule<EventManager>().Publish(new InputEventArgs() { context = context });
        }
    }
}