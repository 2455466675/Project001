using UnityEngine;
using YooAsset;

namespace GameFrameworkAOT.Core 
{
    internal class FsmClearCacheBundle : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            Debug.Log("清理未使用的缓存文件！");
            var packageName = m_Machine.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            operation.Completed += Operation_Completed;
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            m_Machine.ChangeState<FsmFinish>();
        }
    }
}
