using Game.Input;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class BasalInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Basal;

        public BasalInputModule() 
        {
            IsStatic = true;
        }

        protected override void OnInputAction(ActionContext context)
        {
            MLog.Log("BasalInputModule OnInputAction");

            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Esc:
                    GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_1, ModuleType.Panel);
                    break;
            }
        }
    }
}
