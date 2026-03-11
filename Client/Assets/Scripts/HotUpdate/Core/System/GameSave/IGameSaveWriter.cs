namespace GameFramework.Core
{
    public interface IGameSaveWriter : IWriter, IGameSaveEnumerator 
    {
        public IGameSaveData GetData();
    }
}
