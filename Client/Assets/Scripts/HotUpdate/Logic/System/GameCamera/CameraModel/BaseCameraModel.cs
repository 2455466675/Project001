namespace GameFramework.Logic
{
    public enum CameraModel
    {
        Follow,
        Controlled,
    }

    internal abstract class BaseCameraModel
    {
        public abstract CameraModel CameraModel { get; }

        protected CameraNode cameraNode;

        public virtual void Init(CameraNode cameraNode)
        {
            this.cameraNode = cameraNode;
        }
        public abstract void Update();
        public abstract void Enter();
        public abstract void Exit();
    }
}
