using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupComponent : EC.Component
    {
        private NavigationGroup group;

        public UIDefine.Group_ID GroupID => group.groupID;

        public bool IsUndoable { get; private set; }

        public void Init(NavigationGroup group) 
        {
            this.group = group;
            IsUndoable = true;
        }

        public void SetUndoable(bool undoable) 
        {
            IsUndoable = undoable;
        }

        public void Exit() 
        {
            group.OnExit();
        }

        public bool InFocus(bool isRefocus, params int[] indexs)
        {
            return group.OnInFocus(isRefocus, indexs);
        }

        public void OutFocus()
        {
            group.OnOutFocus();
        }

        public void Move(Vector2 dir)
        {
            group.OnMove(dir);
        }

        public void Submit()
        {
            group.OnSubmit();
        }

        protected override void OnDestroy()
        {
            group = null;
        }
    }
}
