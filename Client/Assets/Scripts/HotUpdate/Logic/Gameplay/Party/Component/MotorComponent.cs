using GameFramework.Core;

namespace GameFramework.Logic
{
    public interface IMotorComponent
    {
        void StartUp();
        void ShutDown();
        void SetFollowTarget(int eid);

        void OnInputAction(InputContext context);
    }

    public class MotorComponent : ProjectableComponent<IMotorComponent>, IMotorComponent
    {
        public void StartUp()
        {
            projection.StartUp();
        }

        public void ShutDown()
        {
            projection.ShutDown();
        }

        public void SetFollowTarget(int eid)
        {
            projection.SetFollowTarget(eid);
        }

        public void OnInputAction(InputContext context)
        {
            projection.OnInputAction(context);
        }
    }
}