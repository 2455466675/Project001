using Game.Input;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListCammand : InputCammand
    {
        public NavigationListDefine Define { get; private set; }
        private int[] defaultIndexs;

        public NavigationListCammand(NavigationListDefine define, int[] defaultIndexs = null) : base()
        {
            this.Define = define;
            this.defaultIndexs = defaultIndexs;
        }

        protected override void OnPop()
        {
            GameWorld.Root.GetComponent<UIComponent>().Exit(Define);
        }

        protected override bool OnPush()
        {
            bool r = GameWorld.Root.GetComponent<UIComponent>().InFocus(Define, false, defaultIndexs);
            return r;
        }

        protected override bool OnRise()
        {
            bool r = GameWorld.Root.GetComponent<UIComponent>().InFocus(Define, true);
            return r;
        }

        protected override void OnSink()
        {
            GameWorld.Root.GetComponent<UIComponent>().OutFocus(Define);
        }

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType) 
            {
                case InputType.Move:
                    Vector2 v = context.Vector2Value;
                    GameWorld.Root.GetComponent<UIComponent>().Move(Define, v.x, v.y);
                    break;
                case InputType.Submit:
                    GameWorld.Root.GetComponent<UIComponent>().Submit(Define);
                    break;
            }
        }
    }
}
