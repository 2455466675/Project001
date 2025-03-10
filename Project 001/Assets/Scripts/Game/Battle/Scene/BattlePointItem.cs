using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePointItem : NavigationItem
	{
        public override bool IsValid => CheckValid();

        public BattleActorLoader actorLoader;

        private bool CheckValid()
        {
            return true;
        }
    }
}

