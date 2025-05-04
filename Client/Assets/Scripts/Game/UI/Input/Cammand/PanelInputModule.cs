namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Panel;

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Cancel:
                    Pop();
                    break;
                case InputType.Esc:
                    PopAll();
                    break;
                case InputType.Map:
                    Game.UI.Navigate(NavigationListDefine.Test_List_2, ModuleType.Panel);
                    break;
            }
        }
    }
}
