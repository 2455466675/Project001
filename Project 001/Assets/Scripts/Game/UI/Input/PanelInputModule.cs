using Game.Input;

namespace Game.UI
{
    public class TestGroupCammand : InputCammand 
    {
        protected override void OnInputAction(ActionContext context)
        {
            MLog.Log("TestGroupCammand OnInputAction");
        }
    }

    public class TestListCammand : InputCammand
    {
        public NavigationListDefine id;
        protected override void OnInputAction(ActionContext context)
        {
            MLog.Log("TestListCammand OnInputAction:", id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PanelInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Panel;

        public override void Navigate(NavigationListDefine list_ID)
        {
            MLog.Log("PanelInputModule Navigate");
            Push(new TestGroupCammand());

            if (TryPeek(out InputCammand cammand)) 
            {
                TestListCammand testListCammand = new TestListCammand();
                testListCammand.id = list_ID;
                testListCammand.IsStatic = list_ID == NavigationListDefine.Undefined;
                cammand.Push(testListCammand);
            }
        }

        protected override void OnInputAction(ActionContext context)
        {
            MLog.Log("PanelInputModule OnInputAction");

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
                    GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_2, ModuleType.Panel);
                    break;
            }
        }
    }
}
