using System;
using Cysharp.Threading.Tasks;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Logic
{
    /// <summary>
    /// 云存档
    /// </summary>
    [GameSystem]
    public class CloudSaveSystem : IGameSystem, IInit
    {
        private const int SaveFormatVersion = 1;

        private bool isLoggedIn;
        private ICloudSaveBackend backend;
        private CloudSaveManifest cachedManifest;

        public CloudSaveManifest CachedManifest => cachedManifest;

        void IInit.Init()
        {
            string userId = SystemInfo.deviceUniqueIdentifier;
            backend = new LocalFolderCloudBackend(userId);
        }

        /// <summary>
        /// 替换云后端(平台实现)
        /// </summary>
        public void SetBackend(ICloudSaveBackend customBackend)
        {
            if (customBackend != null)
            {
                backend = customBackend;
                isLoggedIn = false;
                cachedManifest = null;
            }
        }

        public async UniTask<bool> LoginAsync()
        {
            isLoggedIn = await backend.LoginAsync();
            return isLoggedIn;
        }

        /// <summary>
        /// 拉取云端清单
        /// </summary>
        public async UniTask<CloudSaveManifest> FetchManifestAsync()
        {
            if (!await EnsureLoginAsync())
            {
                return null;
            }
            cachedManifest = await backend.FetchManifestAsync();
            return cachedManifest;
        }

        /// <summary>
        /// 上传云存档
        /// </summary>
        /// <param name="localIndex">本地index</param>
        /// <param name="cloudIndex">云端index</param>
        /// <returns>是否上传成功</returns>
        public async UniTask<bool> UploadAsync(int localIndex, int cloudIndex)
        {
            if (!await EnsureLoginAsync())
            {
                return false;
            }

            byte[] blob = Game.GetSystem<GameSaveSystem>().ExportSlotRaw(localIndex);
            if (blob == null || blob.Length == 0)
            {
                MDebug.Warn("CloudSave 上传失败:本地档位无数据, localIndex =", localIndex);
                return false;
            }

            cachedManifest.slots[cloudIndex] = BuildMeta(localIndex, cloudIndex, blob);
            bool ok = await backend.UploadAsync(cloudIndex, blob, cachedManifest);
            if (ok)
            {
                //更新
            }
            return ok;
        }

        /// <summary>
        /// 下载云存档
        /// </summary>
        /// <param name="cloudIndex">云端目标index</param>
        /// <param name="localIndex">本地index</param>
        /// <returns></returns>
        public async UniTask<bool> DownloadAsync(int cloudIndex, int localIndex)
        {
            if (!await EnsureLoginAsync())
            {
                return false;
            }

            byte[] blob = await backend.DownloadAsync(cloudIndex);
            if (blob == null || blob.Length == 0)
            {
                MDebug.Warn("CloudSave 下载失败:云端档位无数据, cloudIndex =", cloudIndex);
                return false;
            }

            GameSaveSystem saveSystem = Game.GetSystem<GameSaveSystem>();
            saveSystem.ImportSlotRaw(localIndex, blob);
            saveSystem.JsonToSummary(localIndex, GetDownloadedSummary(cloudIndex));
            saveSystem.PersistSummary();

            return true;
        }

        public async UniTask<bool> DeleteCloudAsync(int cloudIndex)
        {
            if (!await EnsureLoginAsync())
            {
                return false;
            }

            bool ok = await backend.DeleteAsync(cloudIndex);
            if (ok)
            {
                cachedManifest = await backend.FetchManifestAsync();
            }
            return ok;
        }

        private async UniTask<bool> EnsureLoginAsync()
        {
            if (isLoggedIn)
            {
                return true;
            }
            return await LoginAsync();
        }

        private CloudSaveMeta BuildMeta(int localIndex, int cloudIndex, byte[] blob)
        {
            return new CloudSaveMeta
            {
                cloudIndex = cloudIndex,
                isEmpty = false,
                timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                deviceName = SystemInfo.deviceName,
                version = NextVersion(cloudIndex),
                byteSize = blob.Length,
                checksum = ComputeChecksum(blob),
                saveFormatVersion = SaveFormatVersion,
                displayInfo = Game.GetSystem<GameSaveSystem>().SummaryToJson(localIndex),
            };
        }

        // 覆盖同一云端档位时版本递增,便于以后判断新旧
        private long NextVersion(int cloudIndex)
        {
            if (cachedManifest != null && cachedManifest.slots != null
                && cloudIndex >= 0 && cloudIndex < cachedManifest.slots.Length)
            {
                CloudSaveMeta existing = cachedManifest.slots[cloudIndex];
                if (existing != null && !existing.isEmpty)
                {
                    return existing.version + 1;
                }
            }
            return 1;
        }

        private string GetDownloadedSummary(int cloudIndex)
        {
            if (cachedManifest == null || cachedManifest.slots == null)
            {
                return "";
            }
            if (cloudIndex < 0 || cloudIndex >= cachedManifest.slots.Length)
            {
                return "";
            }

            CloudSaveMeta meta = cachedManifest.slots[cloudIndex];
            if (meta == null || meta.isEmpty)
            {
                return "";
            }

            return meta.displayInfo;
        }

        private static string ComputeChecksum(byte[] bytes)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
