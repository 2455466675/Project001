using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFramework.Core 
{
    internal class FsmRequestPackageVersion : IStateNode
    {
        private StateMachine m_Machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            m_Machine = machine;
        }

        void IStateNode.OnEnter()
        {
            Debug.Log("请求资源版本 !");
            UpdatePackageVersion().Forget();
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }

        private async UniTaskVoid UpdatePackageVersion()
        {
            var packageName = m_Machine.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            await operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
            }
            else
            {
                Debug.Log($"Request package version : {operation.PackageVersion}");
                m_Machine.PackageVersion = operation.PackageVersion;
                m_Machine.ChangeState<FsmUpdatePackageManifest>();
            }
        }
    }
}
