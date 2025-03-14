using Game.Input;

namespace Game
{
    public interface IInputCammand 
    {
        public void OnPop();
        public void OnPush();
        public void OnRise();
        public void OnSink();
    }

    public class InputModule
    {
        public virtual void OnMove(float x, float y) { }
        public virtual void OnSubmit() { }
        public virtual void OnCancel() { }
        public virtual void OnEsc() { }
        public virtual void OnLeftShift() { }
        public virtual void OnMap() { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InputComponent : ECS.Component
    {
        private GameInput gameInput;

        public void Init() 
        {
            gameInput = new GameInput();

            gameInput.Enable();

            var move = Entity.CreateChild<MoveActionWrapper>();
            move.Initialize(gameInput.DefaultMap.Move);
            move.ActionEvent += OnMove;

            var submit = Entity.CreateChild<SubmitActionWrapper>();
            submit.Initialize(gameInput.DefaultMap.Submit);
            submit.ActionEvent += OnSubmit;

            var cancel = Entity.CreateChild<CancelActionWrapper>();
            cancel.Initialize(gameInput.DefaultMap.Cancel);
            cancel.ActionEvent += OnCancel;

            var esc = Entity.CreateChild<EscActionWrapper>();
            esc.Initialize(gameInput.DefaultMap.Esc);
            esc.ActionEvent += OnEsc;

            var ls = Entity.CreateChild<LeftShiftActionWrapper>();
            ls.Initialize(gameInput.DefaultMap.LeftShift);
            ls.ActionEvent += OnLeftShift;

            var map = Entity.CreateChild<MapActionWrapper>();
            map.Initialize(gameInput.DefaultMap.Map);
            map.ActionEvent += OnMap;
        }

        private void OnMove(float x, float y) 
        {
            MLog.Log("OnMove", x, y);
        }
        private void OnSubmit() 
        {
            MLog.Log("OnSubmit");
        }
        private void OnCancel() 
        {
            MLog.Log("OnCancel");
        }
        private void OnEsc() 
        {
            MLog.Log("OnEsc");
        }
        private void OnLeftShift(bool isPress) 
        {
            MLog.Log("OnLeftShift", isPress);
        }
        private void OnMap() 
        {
            MLog.Log("OnMap");
        }
    }
}
