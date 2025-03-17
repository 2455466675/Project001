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
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
        private void OnSubmit() 
        {
            ActionContext context = new()
            {
                InputType = InputType.Submit,
            };
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
        private void OnCancel() 
        {
            ActionContext context = new()
            {
                InputType = InputType.Cancel,
            };
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
        private void OnEsc() 
        {
            ActionContext context = new()
            {
                InputType = InputType.Esc,
            };
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
        private void OnLeftShift(bool isPress) 
        {
            ActionContext context = new()
            {
                InputType = InputType.LeftShift,
                BoolValue = isPress
            };
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
        private void OnMap() 
        {
            ActionContext context = new()
            {
                InputType = InputType.Map,
            };
            GameWorld.Root.GetComponent<UIComponent>().InputAction(context);
        }
    }
}
