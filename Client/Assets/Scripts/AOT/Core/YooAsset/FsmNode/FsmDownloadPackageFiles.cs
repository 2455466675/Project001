using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFrameworkAOT.Core 
{
    public class FsmDownloadPackageFiles : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }

        void IStateNode.OnEnter()
        {
            BeginDownload().Forget();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        private async UniTaskVoid BeginDownload()
        {
            var downloader = m_Machine.Downloader;

            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;

            float MB = 1048576f;
            float sizeMB = Mathf.Clamp(totalDownloadBytes / MB, 0.1f, float.MaxValue);
            Debug.Log($"热更资源数量 : {totalDownloadCount}; 热更资源大小 ：{sizeMB:f1}MB");

            // 需要在下载前检测磁盘空间不足
#if UNITY_EDITOR

#elif UNITY_STANDALONE_WIN
            //DriveInfo drive = new DriveInfo(Path.GetPathRoot(Application.persistentDataPath));
            //if ((totalDownloadBytes + 100 * MB) > drive.AvailableFreeSpace)
            //{
            //    Debug.Log($"剩余存储空间不足");
            //    m_Machine.ChangeState<FsmFinish>();
            //    return;
            //}
#endif

            downloader.DownloadErrorCallback = DownloadError;
            downloader.DownloadUpdateCallback = DownloadUpdate;

            Debug.Log("开始下载资源文件！");
            downloader.BeginDownload();
            await downloader;

            // 下载结果
            if (downloader.Status != EOperationStatus.Succeed)
            {
                Debug.Log("资源下载失败！");
                m_Machine.ChangeState<FsmFinish>();
            }
            else
            {
                m_Machine.ChangeState<FsmDownloadPackageOver>();                
            }
        }

        private void DownloadError(DownloadErrorData data) 
        {
        
        }

        private void DownloadUpdate(DownloadUpdateData data) 
        {

        }
    }
}
