using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFramework.Core 
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
            Debug.Log("开始下载资源文件！");
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
            downloader.DownloadErrorCallback = DownloadError;
            downloader.DownloadUpdateCallback = DownloadUpdate;
            downloader.BeginDownload();
            await downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
            {

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
