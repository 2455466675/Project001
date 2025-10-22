using Cysharp.Threading.Tasks;

namespace GameFramework 
{
    public interface IGameModule
    {
    }

    public interface IGameModule_SyncInit : IGameModule
    {
        void Init();
    }

    public interface IGameModule_AsyncInit : IGameModule
    {
        UniTask Init();
    }

    public interface IUpdate
    {
        void Update();
    }

    public interface IFixedUpdate
    {
        void FixedUpdate();
    }
}
