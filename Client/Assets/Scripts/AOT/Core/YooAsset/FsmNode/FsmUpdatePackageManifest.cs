using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFramework.Core 
{
    public class FsmUpdatePackageManifest : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }

        void IStateNode.OnEnter()
        {
            Debug.Log("更新资源清单！");
            UpdateManifest().Forget();
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }

        private async UniTaskVoid UpdateManifest()
        {
            var packageName = m_Machine.PackageName;
            var packageVersion = m_Machine.PackageVersion;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            await operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
            }
            else
            {
                m_Machine.ChangeState<FsmCreateDownloader>();
            }
        }
    }
}