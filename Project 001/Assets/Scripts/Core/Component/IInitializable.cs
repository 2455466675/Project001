using System.Collections;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInitializable
    {
        IEnumerator Init(GameInitCfg intCfg);
    }
}
