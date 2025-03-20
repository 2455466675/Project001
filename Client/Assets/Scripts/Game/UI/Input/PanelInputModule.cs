using Game.Input;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Panel;

        public override void Navigate(NavigationListDefine list_ID, int[] navigateIndexs)
        {
            NavigationGroupDefine groupDefine = GameWorld.Root.GetComponent<UIComponent>().ListDefineToGroupDefine(list_ID);
            if (groupDefine == NavigationGroupDefine.Undefined)
            {
                return;
            }

            NavigationListCammand listCammand = new NavigationListCammand(list_ID, navigateIndexs);
            NavigationGroupCammand groupCammand;

            if (TryPeek(out InputCammand cammand)) 
            {
                groupCammand = cammand as NavigationGroupCammand;
                if (groupCammand != null && groupCammand.Define == groupDefine) 
                {
                    if (groupCammand.TryPeek(out InputCammand sub)) 
                    {
                        if (sub is NavigationListCammand _sub && _sub.Define == list_ID)
                        {
                            return;
                        }
                    }
                    groupCammand.Push(listCammand);
                    return;
                }                
            }

            groupCammand = new NavigationGroupCammand(groupDefine);
            Push(groupCammand);
            groupCammand.Push(listCammand);
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
