using Cysharp.Threading.Tasks;

namespace GameFramework
{
    public interface IAsyncInit
    {
        UniTask Init();
    }
}
