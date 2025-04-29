
namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActorManager
    {
        public ActorContainer Container { get; private set; }

        public void Init()
        {
            var go = Game.Resource.LoadAndInstantiate(Game.Config.Formula.ActorContainerPath, Game.Root.transform);
            Container = go.GetComponent<ActorContainer>();
        }
    }
}
