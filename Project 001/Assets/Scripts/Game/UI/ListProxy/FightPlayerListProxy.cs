using Cysharp.Threading.Tasks;
using Game.UI;
using Navigation;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightPlayerListProxy : ListProxy
    {
        public override ListName Name => ListName.FightPlayerList;

        public override WindowId WindowId => WindowId.WinFightBg;

        public override async UniTask Precondition()
        {
            Window window = await GameCore.UI.ShowWindowAsync(WindowId);
            parent = window;
            list = NavigationList.GetNavigationList(ListName.FightPlayerList);
        }
    }
}

