namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class BasalInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Basal;

        protected override void OnSink()
        {
            Game.System.PartySystem.Move(0f, 0f);
            Game.System.PartySystem.Run(false);
        }

        public override void Navigate(NavigationListDefine list_ID, int[] navigateIndexs)
        {
            MLog.Error("BasalInputModule输入模式不支持此功能");
        }

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Esc:
                    Game.UI.Navigate(NavigationListDefine.Overview_Menu_List, ModuleType.Panel);
                    break;
                case InputType.Move:
                    Game.System.PartySystem.Move(context.Vector2Value.x, context.Vector2Value.y);
                    break;
                case InputType.LeftShift:
                    Game.System.PartySystem.Run(context.BoolValue);
                    break;
                case InputType.GM:
                    Game.UI.Navigate(NavigationListDefine.GM_Menu_List, ModuleType.Panel);
                    break;
                case InputType.Map:
                    Game.UI.Navigate(NavigationListDefine.Battle_Enemy_Unit_List, ModuleType.Battle);
                    break;
            }
        }

        protected override bool CheckIsLocked()
        {
            return true;
        }
    }
}
