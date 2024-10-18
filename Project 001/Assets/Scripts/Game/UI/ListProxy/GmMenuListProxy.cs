using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class GmMenuListProxy : ListProxy
    {
        public override ListName Name => ListName.GmMenuList;

        public override WindowId WindowId => WindowId.WinGM;
        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }
    }
}

