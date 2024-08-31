using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightSystem : IGameSystem
	{
        public ListDB<FightPointItemDB> EnemyPoints;

        public ListDB<FightPointItemDB> PlayerPoints;

        public FightPointItemDB ActionPoint;
    }
}

