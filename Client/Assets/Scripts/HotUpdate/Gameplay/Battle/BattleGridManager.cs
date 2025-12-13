using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public static class TileState
    {
        public const int Green = 0;
        public const int Red = 1;
        public const int Yellow = 2;
        public const int Blue = 3;
    }

    public class TileMask
    {
        public int layer;
        public int state;
    }

    public class TileItem : DataModelWrapper
    {
        private Stack<int> states = new Stack<int>();

        private Stack<TileMask> masks = new Stack<TileMask>();

        public void PushState(int state)
        {
            states.Push(state);
            UpdateState();
        }

        public void PushState(int layer, int state)
        {
            TileMask mask = null;
            if (masks.TryPeek(out TileMask m))
            {
                if (layer < m.layer)
                {
                    return;
                }

                if (layer == m.layer)
                {
                    mask = m;
                }
            }

            if (mask == null)
            {
                mask = new TileMask();
                mask.layer = layer;
                masks.Push(mask);
            }

            mask.state = state;

            UpdateState();
        }

        public void PopState(int layer)
        {
            if (masks.TryPeek(out TileMask m))
            {
                if (layer > m.layer)
                {
                    return;
                }

                while (true)
                {
                    m = masks.Pop();
                    if (m.layer <= layer)
                    {
                        break;
                    }
                }

                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (masks.TryPeek(out TileMask mask))
            {
                SetValue(DataKey.State, mask.state);
            }
        }

        public bool Passable()
        {
            int battleId = GetIntValue(DataKey.BattleId);
            return battleId <= 0;
        }
    }

    public class BattleGridManager
    {
        private List<TileItem> tiles;

        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

        private Stack<int> layers;

        public BattleGridManager()
        {
            tiles = new List<TileItem>();
            layers = new Stack<int>();
        }

        public void InitGrid(int r, int c)
        {
            RowCount = r; 
            ColCount = c;

            BattleUtils.RowCount = r;
            BattleUtils.ColCount = c;

            layers.Clear();
            tiles.Clear();

            List<Vector2Int> points = new List<Vector2Int>();

            int count = r * c;
            for (int i = 0; i < count; i++)
            {
                TileItem tile = new TileItem();
                tile.SetValue(DataKey.Index, i);
                tile.SetValue(DataKey.PosX, 0f);
                tile.SetValue(DataKey.PosZ, 0f);
                tiles.Add(tile);

                points.Add(BattleUtils.Index2Coord(i));
            }

            Draw(0, TileState.Green, points);
        }

        public IReadOnlyList<DataModel> GetTiles()
        {
            List<DataModel> list = new List<DataModel>(tiles.Count);

            for (int i = 0; i < tiles.Count; i++)
            {
                list.Add(tiles[i].GetDataModel());
            }

            return list;
        }

        public TileItem GetTile(int index)
        {
            if (index < 0 || index >= tiles.Count)
            {
                return null;
            }
            else
            {                
                return tiles[index]; 
            }
        }

        public TileItem GetTile(int coordX, int coordY)
        {
            int index = BattleUtils.Coord2Index(coordX, coordY);
            return GetTile(index);
        }

        public void Draw(int layer, int state, List<Vector2Int> coords)
        {
            MDebug.Log("Draw", layer, "--", coords.Count);
            if (coords == null || coords.Count == 0)
            {
                return;
            }

            if (layers.TryPeek(out int l))
            {
                if (layer < l)
                {
                    return;
                }

                if (layer > l)
                {
                    layers.Push(layer);
                }
            }
            else
            {
                layers.Push(layer);
            }

            foreach (var item in coords)
            {
                int x = item.x;
                int y = item.y;
                var tile = GetTile(x, y);
                tile?.PushState(layer, state);
            }
        }

        public void Wipe()
        {
            MDebug.Log("Wipe");
            while (true)
            {
                if (layers.TryPeek(out int l))
                {
                    if (l <= 0)
                    {
                        break;
                    }

                    foreach (var item in tiles)
                    {
                        item.PopState(l);
                    }
                    layers.Pop();
                }
                else
                {
                    break;
                }
            }
        }

        public void Wipe(int layer)
        {
            if (layers.TryPeek(out int l))
            {
                if (layer <= l)
                {
                    layers.Pop();
                    foreach (var item in tiles)
                    {
                        item.PopState(l);
                    }
                }           
            }
        }
    }
}
