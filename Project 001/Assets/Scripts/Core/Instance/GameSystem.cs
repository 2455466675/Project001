using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSystem
    {
        public static GameSystem Inst { get; private set; }

        public ItemSystem ItemSystem { get; private set; }

        public GameSystem()
        {
            Inst = this;
            ItemSystem = new ItemSystem();
        }
    }
}