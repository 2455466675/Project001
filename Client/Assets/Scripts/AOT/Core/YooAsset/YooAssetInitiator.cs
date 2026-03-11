using YooAsset;

namespace GameFrameworkAOT.Core 
{
    public class YooAssetInitiator : GameAsyncOperation
    {
        private enum ESteps
        {
            None,
            Update,
            Done,
        }

        private ESteps m_Steps = ESteps.None;
        private readonly StateMachine m_Machine;

        public YooAssetInitiator(string packageName, EPlayMode playMode) 
        {
            m_Machine = new StateMachine(this);
            m_Machine.AddNode<FsmInitializePackage>();
            m_Machine.AddNode<FsmRequestPackageVersion>();
            m_Machine.AddNode<FsmUpdatePackageManifest>();
            m_Machine.AddNode<FsmCreateDownloader>();
            m_Machine.AddNode<FsmDownloadPackageFiles>();
            m_Machine.AddNode<FsmDownloadPackageOver>();
            m_Machine.AddNode<FsmClearCacheBundle>();
            m_Machine.AddNode<FsmFinish>();

            m_Machine.PackageName = packageName;
            m_Machine.PlayMode = playMode;

            YooAssets.Initialize();
            YooAssets.StartOperation(this);
        }

        protected override void OnStart()
        {
            m_Steps = ESteps.Update;
            m_Machine.Run<FsmInitializePackage>();
        }

        protected override void OnUpdate()
        {
            if (m_Steps == ESteps.None || m_Steps == ESteps.Done)
                return;

            if (m_Steps == ESteps.Update)
            {
                m_Machine.Update();
            }
        }

        protected override void OnAbort()
        {

        }

        public void SetFinish() 
        {
            m_Steps = ESteps.Done;
            Status = EOperationStatus.Succeed;
        }
    }
}