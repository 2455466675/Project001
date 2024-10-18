using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class GmItemListProxy : ListProxy
	{
        public override ListName Name => ListName.GmItemList;

        public override WindowId WindowId => WindowId.WinGM;
        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }
    }
}

