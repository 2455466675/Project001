using Game.Input;
using Game.UI;

namespace Game
{    
    /// <summary>
    /// 
    /// </summary>
    public class InputComponent : ECS.Entity
    {
        private GameInput gameInput;

        public void Init() 
        {
            gameInput = new GameInput();

            gameInput.Enable();

            var move = CreateChild<MoveActionWrapper>();
            move.Initialize(gameInput.DefaultMap.Move);
            move.ActionEvent += OnMove;

            var submit = CreateChild<SubmitActionWrapper>();
            submit.Initialize(gameInput.DefaultMap.Submit);
            submit.ActionEvent += OnSubmit;

            var cancel = CreateChild<CancelActionWrapper>();
            cancel.Initialize(gameInput.DefaultMap.Cancel);
            cancel.ActionEvent += OnCancel;

            var esc = CreateChild<EscActionWrapper>();
            esc.Initialize(gameInput.DefaultMap.Esc);
            esc.ActionEvent += OnEsc;

            var ls = CreateChild<LeftShiftActionWrapper>();
            ls.Initialize(gameInput.DefaultMap.LeftShift);
            ls.ActionEvent += OnLeftShift;

            var map = CreateChild<MapActionWrapper>();
            map.Initialize(gameInput.DefaultMap.Map);
            map.ActionEvent += OnMap;
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

        private void DoAction(ActionContext context) 
        {
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
    }
}
