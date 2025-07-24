using Game.UI.Input;
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

        public NavigationListCammand(NavigationListDefine define, int[] defaultIndexs) : base()
        {
            this.Define = define;
            this.defaultIndexs = defaultIndexs;
        }

        protected override void OnPop()
        {
            var entity = GetEntity();
            entity?.Exit();            
        }

        protected override bool OnPush()
        {
            var entity = GetEntity();
            if (entity == null) 
            {
                return false;
            }
            else
            {
                return entity.InFocus(false, defaultIndexs);
            }          
        }

        protected override bool OnRise()
        {
            if (IsPopAll) //isPopAll不再聚焦，必定成功
            {
                return true;
            }

            var entity = GetEntity();
            if (entity == null)
            {
                return false;
            }
            else
            {
                return entity.InFocus(true);
            }
        }

        protected override void OnSink()
        {
            var entity = GetEntity();
            entity?.OutFocus();
        }

        protected override void OnInputAction(ActionContext context)
        {
            var entity = GetEntity();

            InputType inputType = context.InputType;
            switch (inputType) 
            {
                case InputType.Move:
                    Vector2 v = context.Vector2Value;
                    entity?.Move(v.x, v.y);
                    break;
                case InputType.Submit:
                    entity?.Submit();                
                    break;
            }
        }

        protected override bool CheckIsLocked()
        {
            var entity = GetEntity();
            return entity != null && entity.IsLocked;
        }

        private NavigationListEntity GetEntity() 
        {
            return Game.UI.GetNavigationListEntity(Define);
        }
    }
}
