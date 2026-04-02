namespace ECS
{
    public interface ILateUpdateableComponent
    {
        public void LateUpdate(float deltaTime);
    }
}