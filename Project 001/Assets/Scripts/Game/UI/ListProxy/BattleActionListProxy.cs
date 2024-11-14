using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleActionListProxy : ListProxy
    {
        public override ListName Name => ListName.BattleActionList;

        public override WindowId WindowId => WindowId.WinBattleAction;

        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }

        public override bool InFocus(params int[] indexs)
        {
            list.SetAlpha(1f);
            return base.InFocus(indexs);
        }

        public override bool IsUndoable()
        {
            return false;
        }
    }
}

