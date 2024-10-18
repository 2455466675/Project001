using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageListListProxy : ListProxy
    {
        public override ListName Name => ListName.PackageList;

        public override WindowId WindowId => WindowId.WinPackage;

        public override async UniTask Precondition()
        {
            await LoadWindowAsync();
        }
    }
}

