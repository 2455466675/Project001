using Game.UI;
using System.Collections;
using System.Collections.Generic;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleNavigationItem : GameNavigationItem
    {
        public override bool IsValid 
        {
            get 
            {
                if (TryGetData(out BattleUnit unit)) 
                {
                    return unit.State == BattleUnitState.Alive;
                }

                return false;
            }
        }
    }
}
