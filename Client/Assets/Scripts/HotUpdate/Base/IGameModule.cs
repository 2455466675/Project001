using Cysharp.Threading.Tasks;

namespace GameFramework 
{
    public interface IGameModule
    {
    }

    public interface ISyncInit : IGameModule
    {
        void Init();
    }

    public interface IAsyncInit : IGameModule
    {
        UniTask Init();
    }

    public interface IUpdate
    {
        void Update();
    }
}
