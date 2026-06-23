using YooAsset;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    internal class AssetRequester
    {
        private UniTaskCompletionSource<AssetHandle> source;
        public UniTask Task => source.Task;
        public AssetHandle Handle { get; set; }

        internal AssetRequester()
        {
            source = new UniTaskCompletionSource<AssetHandle>();
        }

        public void SetResult(AssetHandle handle)
        {
            source.TrySetResult(handle);
        }
    }
}
