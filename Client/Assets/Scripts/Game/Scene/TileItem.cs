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
        [SerializeField]
        private SpriteRenderer borderTile;

        public int RowIndex { get; private set; }
        public int ColIndex { get; private set; }

        public void SetIndex(int rIndex, int cIndex) 
        {
            RowIndex = rIndex; 
            ColIndex = cIndex;
            name = string.Format("TileItem:({0},{1})", rIndex, cIndex);
        }

        public void SetState(bool state) 
        {
            borderTile.gameObject.SetActive(state);
        }
    }
}