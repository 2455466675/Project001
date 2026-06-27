using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    /// <summary>
    /// 云存档传输后端抽象。所有平台相关实现(本地目录假后端、UGS、Steam、自建后端…)都收敛在此接口背后,
    /// 更换平台只需新增实现并注入,上层编排与本地存档系统零改动。
    /// 接口按云端档位索引(cloudIndex)寻址,只进出不透明的 byte[] blob 与 CloudSaveMeta。
    /// </summary>
    public interface ICloudSaveBackend
    {
        UniTask<bool> LoginAsync();

        UniTask<CloudSaveManifest> FetchManifestAsync();

        UniTask<bool> UploadAsync(int cloudIndex, byte[] blob, CloudSaveManifest manifest);

        // 云端档位为空时返回 null
        UniTask<byte[]> DownloadAsync(int cloudIndex);

        UniTask<bool> DeleteAsync(int cloudIndex);
    }
}
