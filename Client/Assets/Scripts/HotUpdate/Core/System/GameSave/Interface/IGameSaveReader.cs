namespace GameFramework.Core
{
    public interface IGameSaveReader : IReader, IGameSaveEnumerator
    {
        public void SetData(IGameSaveData data);
    }
}