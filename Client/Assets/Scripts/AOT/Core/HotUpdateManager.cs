using System.Collections;
using GameFrameworkAOT.Core;

namespace GameFrameworkAOT
{
    public class HotUpdateManager
    {
        private IEnumerator enumerator;

        public HotUpdateManager(GameInitConfig config)
        {
            enumerator = new YooAssetInitiator(config.packageName, config.playMode);
        }

        public IEnumerator Start()
        {
            yield return enumerator;
        }
    }
}
