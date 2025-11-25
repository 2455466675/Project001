using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System.Collections.Generic;
using System.Linq;
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

    public class TileItem : DataModelWrapper
    {
        private Stack<int> states = new Stack<int>();

        public void PushState(int state)
        {
            states.Push(state);
            UpdateState();
        }

        public void PopState()
        {
            if (states.Count <= 1)
            {
                return;
            }
            states.Pop();
            UpdateState();
        }

        private void UpdateState()
        {
            if (states.TryPeek(out int state))
            {
                SetValue("state", state);
            }
        }

        public bool Passable()
        {
            return true;
        }
    }

    public class BattleGridManager
    {
        private static readonly List<(int, int)> neighbor = new List<(int, int)>()
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0),
        };

        private class Node
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int G { get; private set; }
            public int H { get; private set; }
            public int F => G + H;
            public Node Parent { get; private set; }

            public Node(int x, int y, int g, int h, Node parent)
            {
                X = x;
                Y = y;
                G = g;
                H = h;
                Parent = parent;
            }
        }

        private List<TileItem> tiles;

        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

        public BattleGridManager()
        {
            tiles = new List<TileItem>();
        }

        public void InitGrid(int r, int c)
        {
            RowCount = r; 
            ColCount = c;

            tiles.Clear();
            int count = r * c;
            for (int i = 0; i < count; i++)
            {
                TileItem tile = new TileItem();
                tile.SetValue("index", i);
                tile.SetValue("posX", 0f);
                tile.SetValue("posZ", 0f);
                tile.PushState(TileState.Green);
                tiles.Add(tile);
            }
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
            int index = Coord2Index(coordX, coordY);
            return GetTile(index);
        }

        public void SelectTile(int index)
        {
            var tile = GetTile(index);
            tile?.PushState(TileState.Red);
        }

        public void DeselectTile(int index)
        {
            var tile = GetTile(index);
            tile?.PopState();
        }

        public void DrawTiles(int x, int y, int r, int state)
        {
            var points = GetCirclePoints(x, y, r);

            foreach (var item in points)
            {
                var tile = GetTile(item.Item1, item.Item2);
                tile?.PushState(state);
            }
        }

        public static List<(int, int)> GetCirclePoints(int a, int b, int n)
        {
            var points = new HashSet<(int, int)>();

            if (n == 0)
            {
                points.Add((a, b));
                return points.ToList();
            }

            for (int i = 1; i <= n; i++)
            {
                for (int dx = -i; dx <= i; dx++)
                {
                    int rd = i - Utility.Math.Abs(dx);
                    if (rd == 0)
                    {
                        points.Add((a + dx, b));
                    }
                    else
                    {
                        points.Add((a + dx, b + rd));
                        points.Add((a + dx, b - rd));
                    }
                }
            }

            return points.ToList();
        }

        public Vector3 Coord2Pos(int coordX, int coordY)
        {
            int index = Coord2Index(coordX, coordY);
            TileItem tile = GetTile(index);
            if (tile == null)
            {
                return Vector3.zero;
            }
            else
            {
                float x = tile.GetFloatValue("posX");
                float z = tile.GetFloatValue("posZ");
                return new Vector3(x, 0.05f, z);
            }
        }

        public int Coord2Index(int x, int y)
        {
            if (x < 0 || x >= ColCount)
            {
                return -1; 
            }
            if (y < 0 || y >= RowCount) 
            {
                return -1; 
            }
            return y * RowCount + x;
        }

        public Vector2Int Index2Coord(int index)
        {
            int x = index % ColCount;
            int y = index / ColCount;
            return new Vector2Int(x, y);
        }

        private void ResetState()
        {
            foreach (var item in tiles)
            {
                item.PopState();
            }
        }

        public List<Vector2Int> AStarPath(int startX, int startY, int targetX, int targetY)
        {
            if (!CheckIndexValid(startX, startY) || !CheckIndexValid(targetX, targetY))
            {
                return new List<Vector2Int>();
            }

            bool[,] closeList = new bool[ColCount, RowCount];
            List<Node> openList = new List<Node>();

            Node startNode = new Node(startX, startY, 0, H_Value(startX, startY, targetX, targetY), null);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currNode = openList.Aggregate((min, next) => next.F < min.F ? next : min);
                openList.Remove(currNode);

                int x = currNode.X;
                int y = currNode.Y;

                if (closeList[x, y])
                {
                    continue;
                }
                else
                {
                    closeList[x, y] = true;
                }

                if (x == targetX && y == targetY)
                {
                    return ReconstructPath(currNode);
                }

                for (int i = 0; i < neighbor.Count; i++)
                {
                    var dir = neighbor[i];
                    int nx = x + dir.Item1;
                    int ny = y + dir.Item2;

                    if (!CheckIndexValid(nx, ny))
                    {
                        continue;
                    }

                    if (closeList[nx, ny])
                    {
                        continue;
                    }

                    Node neighborNode = new Node(nx, ny, currNode.G + 1, H_Value(nx, ny, targetX, targetY), currNode);
                    openList.Add(neighborNode);
                }
            }
            return new List<Vector2Int>();
        }

        private bool CheckIndexValid(int x, int y)
        {
            if (x < 0 || x >= ColCount || y < 0 || y >= RowCount)
            {
                return false;
            }

            TileItem item = GetTile(x, y);
            if (item == null)
            {
                return false;
            }

            return item.Passable();
        }

        private int H_Value(int startX, int startY, int targetX, int targetY)
        {
            return Utility.Math.Abs(startX - targetX) + Utility.Math.Abs(startY - targetY);
        }

        private List<Vector2Int> ReconstructPath(Node node)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (node != null)
            {
                path.Add(new Vector2Int(node.X, node.Y));
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}
