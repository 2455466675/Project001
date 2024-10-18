using Navigation;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightPointItem : GuidableItem
	{
        public override bool IsValid => CheckValid();

        private bool CheckValid()
        {
            return Datum.GetDataBase("valid").BoolValue;
        }
    }
}

