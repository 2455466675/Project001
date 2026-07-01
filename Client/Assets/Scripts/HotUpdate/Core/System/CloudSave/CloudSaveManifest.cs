using System;

namespace GameFramework.Core
{
    /// <summary>
    /// 云端存档清单:固定数量的档位概览,是本地 summary 在云端的镜像。
    /// 玩家"打开云端列表"时拉取它来选择要操作的云端档位。
    /// </summary>
    [Serializable]
    public class CloudSaveManifest
    {
        public const int CLOUD_SLOT_COUNT = 10;

        public string userId;
        public CloudSaveMeta[] slots;

        public static CloudSaveManifest CreateEmpty(string userId, int slotCount)
        {
            CloudSaveManifest manifest = new CloudSaveManifest
            {
                userId = userId,
                slots = new CloudSaveMeta[slotCount],
            };
            for (int i = 0; i < slotCount; i++)
            {
                manifest.slots[i] = new CloudSaveMeta
                {
                    cloudIndex = i,
                    isEmpty = true,
                };
            }
            return manifest;
        }
    }
}
