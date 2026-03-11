namespace GameFramework.Core
{
    public interface IGameSavable
    {
        void OnSaveGame(IWriter writer);
        void OnLoadGame(IReader reader);
    }
}