namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class BasalInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Basal;

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Esc:
                   // GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_1, ModuleType.Panel);
                    break;
                case InputType.Move:
                    Game.System.RoleSystem.Move(context.Vector2Value.x, context.Vector2Value.y);
                    break;
                case InputType.LeftShift:
                    Game.System.RoleSystem.Run(context.BoolValue);
                    break;
            }
        }

        protected override bool CheckIsLocked()
        {
            return true;
        }
    }
}
