using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightPointItem : GuidableItem<FightPointItemDB>
	{
        public override bool IsValid { get => base.IsValid; protected set => base.IsValid = value; }
        protected override void OnDatumChange()
        {
            
        }
    }
}

