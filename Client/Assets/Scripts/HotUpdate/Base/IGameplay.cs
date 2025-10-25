using Cysharp.Threading.Tasks;

namespace GameFramework 
{
    public interface IGameplay
    {
        UniTask Init();
        void Exit();
        void SaveGame(ISaveWriter writer);
        void LoadGame(ISaveReader reader);
        T GetSystem<T>() where T : class, IGameplaySystem;
    }

    public interface IGameplaySystem 
    {
        void OnInit();
        void OnExit();
        void OnSaveGame(ISaveWriter writer);
        void OnLoadGame(ISaveReader reader);
    }
}