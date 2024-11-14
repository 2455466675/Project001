using Navigation;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePointItem : GuidableItem
	{
        public override bool IsValid => CheckValid();

        public BattleActorLoader actorLoader;

        private bool CheckValid()
        {
            return Datum.GetDataBase("valid").BoolValue;
        }
    }
}

