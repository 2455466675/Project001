using UnityEngine;
using YooAsset;

namespace GameFrameworkAOT.Core 
{
    public class FsmCreateDownloader : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }

        void IStateNode.OnEnter()
        {
            Debug.Log("创建资源下载器！");
            CreateDownloader();
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }

        void CreateDownloader()
        {
            var packageName = m_Machine.PackageName;
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            m_Machine.Downloader = downloader;

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                m_Machine.ChangeState<FsmFinish>();
            }
            else
            {
                m_Machine.ChangeState<FsmDownloadPackageFiles>();
            }
        }
    }
}
