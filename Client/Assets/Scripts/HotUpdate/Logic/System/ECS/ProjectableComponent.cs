using ECS;

namespace GameFramework.Logic
{
    public interface IProjectable 
    {
        void Project();
        void Reflect(IProjection projection);
    }

    public interface IProjection
    {

    }


    public class ProjectableComponent<T> : ComponentBase, IProjectable
    {
        protected T projection;

        protected override void Awake()
        {
            ((IProjectable)this).Project();
        }

        void IProjectable.Project()
        {
            var type = this.GetType();
            Game.Message.SendMessage(new ProjectComponentMessage() { eid = Eid, type = type });
        }

        void IProjectable.Reflect(IProjection projection)
        {
            this.projection = (T)projection;
        }
    }
}
