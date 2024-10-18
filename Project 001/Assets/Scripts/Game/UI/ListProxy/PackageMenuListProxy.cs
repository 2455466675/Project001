using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageMenuListProxy : ListProxy
    {
        public override ListName Name => ListName.PackageMenu;

        public override WindowId WindowId => WindowId.WinPackage;
        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }

    }
}

