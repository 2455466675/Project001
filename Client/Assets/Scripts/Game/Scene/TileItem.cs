using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class TileItem : GameNavigationItem
    {
        [SerializeField]
        private SpriteRenderer baseTile;
        [SerializeField]
        private SpriteRenderer redTile;
        [SerializeField]
        private SpriteRenderer yellowTile;
        [SerializeField]
        private SpriteRenderer blueTile;     
    }
}