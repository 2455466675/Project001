using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    public interface IBattlePhase
    {
        UniTask Run(IBattleContext context);
    }
}
