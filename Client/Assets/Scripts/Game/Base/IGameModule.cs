using Cysharp.Threading.Tasks;

namespace GameFramework 
{
    public interface IGameModule
    {
        GameModulePriority Priority { get; }

        UniTask Init();
    }

    public interface IUpdate
    {
        void Update();
    }
}
