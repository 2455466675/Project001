using Game.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game 
{
    [Flags]
    public enum MoveType
    {            
        None   = 0,
        Walk   = 1,
        Fly    = 1 << 1,
    }

    public enum TitleState 
    {
        Normal = 0,
        Red = 1,
        Yellow = 2,
        Blue = 3,
    }

    public class TileItem : GameNavigationItem
    {
        [SerializeField]
        private MoveType mask;

        [SerializeField]
        private SpriteRenderer borderTile;
        [SerializeField]
        private SpriteRenderer baseTile;
        [SerializeField]
        private SpriteRenderer redTile;
        [SerializeField]
        private SpriteRenderer yellowTile;
        [SerializeField]
        private SpriteRenderer blueTile;
        [ReadOnly]
        [SerializeField]
        private int x;
        [ReadOnly]
        [SerializeField]
        private int y;

        public int X => x;
        public int Y => y;

        public void SetIndex(int x, int y) 
        {
            this.x = x; 
            this.y = y;
            name = string.Format("TileItem:({0},{1})", x, y);
        }

        public bool Passable(MoveType moveType) 
        {
            return (mask & moveType) != 0;
        }

        public void Focus(bool focus) 
        {
            borderTile.gameObject.SetActive(focus);
        }

        public void SetState(TitleState state) 
        {
            switch (state)
            {
                case TitleState.Normal:
                    baseTile.gameObject.SetActive(true);
                    redTile.gameObject.SetActive(false);
                    yellowTile.gameObject.SetActive(false);
                    blueTile.gameObject.SetActive(false);
                    break;
                case TitleState.Red:
                    baseTile.gameObject.SetActive(false);
                    redTile.gameObject.SetActive(true);
                    yellowTile.gameObject.SetActive(false);
                    blueTile.gameObject.SetActive(false);
                    break;
                case TitleState.Yellow:
                    baseTile.gameObject.SetActive(false);
                    redTile.gameObject.SetActive(false);
                    yellowTile.gameObject.SetActive(true);
                    blueTile.gameObject.SetActive(false);
                    break;
                case TitleState.Blue:
                    baseTile.gameObject.SetActive(false);
                    redTile.gameObject.SetActive(false);
                    yellowTile.gameObject.SetActive(false);
                    blueTile.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}