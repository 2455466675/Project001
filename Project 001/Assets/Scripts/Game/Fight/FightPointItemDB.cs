using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightPointItemDB : ItemDB
    {
        public int index;
        public override int CompareTo(ItemDB db)
        {
            FightPointItemDB fDB = db as FightPointItemDB;
            return fDB.index.CompareTo(index);
        }
    }
}

