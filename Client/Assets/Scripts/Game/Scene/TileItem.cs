using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class TileItem : NavigationItem
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