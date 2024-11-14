using Cysharp.Threading.Tasks;
using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleActionList2Proxy : ListProxy
    {
        public override ListName Name => ListName.BattleActionList2;

        public override WindowId WindowId => WindowId.WinBattleAction2;

        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }

        public override bool InFocus(params int[] indexs)
        {
            list.SetAlpha(1f);
            return base.InFocus(indexs);
        }

        public override bool OutFocus()
        {
            list.SetAlpha(0f);

            NavigationList fightActionList = NavigationList.GetNavigationList(ListName.BattleActionList);
            fightActionList.SetAlpha(0f);

            return base.OutFocus();
        }

        public override bool Refocus()
        {
            list.SetAlpha(1f);

            NavigationList fightActionList = NavigationList.GetNavigationList(ListName.BattleActionList);
            fightActionList.SetAlpha(1f);

            return base.Refocus();
        }

        public override void Exit()
        {
            list.SetAlpha(0f);
            base.Exit();
        }
    }
}

