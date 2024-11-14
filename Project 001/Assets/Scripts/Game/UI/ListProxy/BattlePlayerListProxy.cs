using Cysharp.Threading.Tasks;
using Game.UI;
using Navigation;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePlayerListProxy : ListProxy
    {
        public override ListName Name => ListName.BattlePlayerList;

        public override WindowId WindowId => WindowId.WinBattleBg;

        public override async UniTask Precondition()
        {
            Window window = await GameCore.UI.ShowWindowAsync(WindowId);
            parent = window;
            list = NavigationList.GetNavigationList(ListName.BattlePlayerList);
        }
    }
}

