using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Core
{
    /// <summary>
    /// 把"云端"模拟成本地一个独立目录的假后端。用于测试期跑通整条上传/下载链路,零外部依赖、零账号配置。
    /// 它和游戏正常存档目录分开存放,语义上代表"另一台机器/远端",以便真实地测试跨档覆盖、下载回写等流程。
    /// 之后换真云只需另写一个 ICloudSaveBackend 实现替换它。
    /// </summary>
    public class LocalFolderCloudBackend : ICloudSaveBackend
    {
        private const string ManifestFileName = "manifest.json";

        private readonly string userId;
        private readonly int slotCount;
        private readonly string rootDir;

        public LocalFolderCloudBackend(string userId, int slotCount = CloudSaveManifest.CLOUD_SLOT_COUNT, string rootOverride = null)
        {
            this.userId = userId;
            this.slotCount = slotCount;

            string baseDir = string.IsNullOrEmpty(rootOverride)
                ? Path.Combine(Application.persistentDataPath, "CloudSim")
                : rootOverride;
            rootDir = Path.Combine(baseDir, userId);
        }

        public UniTask<bool> LoginAsync()
        {
            EnsureRoot();
            return UniTask.FromResult(true);
        }

        public UniTask<CloudSaveManifest> FetchManifestAsync()
        {
            return UniTask.FromResult(LoadManifest());
        }

        public UniTask<bool> UploadAsync(int cloudIndex, byte[] blob, CloudSaveManifest manifest)
        {
            if (!IsValidIndex(cloudIndex) || blob == null || blob.Length == 0)
            {
                return UniTask.FromResult(false);
            }

            EnsureRoot();

            //async 上传存档内容
            File.WriteAllBytes(BlobPath(cloudIndex), blob);
            //async 上传云清单
            SaveManifest(manifest);

            return UniTask.FromResult(true);
        }

        public UniTask<byte[]> DownloadAsync(int cloudIndex)
        {
            if (!IsValidIndex(cloudIndex))
            {
                return UniTask.FromResult<byte[]>(null);
            }

            string path = BlobPath(cloudIndex);
            if (!File.Exists(path))
            {
                return UniTask.FromResult<byte[]>(null);
            }
            return UniTask.FromResult(File.ReadAllBytes(path));
        }

        public UniTask<bool> DeleteAsync(int cloudIndex)
        {
            if (!IsValidIndex(cloudIndex))
            {
                return UniTask.FromResult(false);
            }

            string path = BlobPath(cloudIndex);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            CloudSaveManifest manifest = LoadManifest();
            manifest.slots[cloudIndex] = new CloudSaveMeta
            {
                cloudIndex = cloudIndex,
                isEmpty = true,
            };
            SaveManifest(manifest);

            return UniTask.FromResult(true);
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < slotCount;
        }

        private void EnsureRoot()
        {
            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
        }

        private string BlobPath(int index)
        {
            return Path.Combine(rootDir, $"slot_{index}.bin");
        }

        private string ManifestPath()
        {
            return Path.Combine(rootDir, ManifestFileName);
        }

        private CloudSaveManifest LoadManifest()
        {
            string path = ManifestPath();
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                CloudSaveManifest manifest = JsonUtility.FromJson<CloudSaveManifest>(json);
                if (manifest != null && manifest.slots != null && manifest.slots.Length == slotCount)
                {
                    return manifest;
                }
            }
            return CloudSaveManifest.CreateEmpty(userId, slotCount);
        }

        private void SaveManifest(CloudSaveManifest manifest)
        {
            EnsureRoot();
            File.WriteAllText(ManifestPath(), JsonUtility.ToJson(manifest, true));
        }
    }
}
