using GameFramework.Featrue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public static class BattleUtils
    {
        private static readonly List<Vector2Int> neighbor = new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
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

        public static int RowCount { get; set; }
        public static int ColCount { get; set; }

        /// <summary>
        /// 获取曼哈顿距离范围
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="r">距离</param>
        /// <returns></returns>
        public static List<Vector2Int> GetManhattanCirclePoints(int x, int y, int radius)
        {
            var points = new HashSet<Vector2Int>();
            points.Add(new Vector2Int(x, y));
            if (radius > 0)
            {
                for (int i = 1; i <= radius; i++)
                {
                    for (int dx = -i; dx <= i; dx++)
                    {
                        int rd = i - Utility.Math.Abs(dx);
                        if (rd == 0)
                        {
                            points.Add(new Vector2Int(x + dx, y));
                        }
                        else
                        {
                            points.Add(new Vector2Int(x + dx, y + rd));
                            points.Add(new Vector2Int(x + dx, y - rd));
                        }
                    }
                }
            }

            return points.ToList();
        }

        /// <summary>
        /// 获取曼哈顿距离范围（检测点的可达性）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <param name="IsPassable"></param>
        /// <returns></returns>
        public static List<Vector2Int> GetManhattanCircleAccessiblePoints(int x, int y, int radius, Func<Vector2Int, bool> IsPassable)
        {
            return GetManhattanCircleAccessiblePoints(new Vector2Int(x, y), radius, IsPassable);
        }

        /// <summary>
        /// 获取曼哈顿距离范围（检测点的可达性）
        /// </summary>
        /// <param name="start"></param>
        /// <param name="radius"></param>
        /// <param name="IsPassable"></param>
        /// <returns></returns>
        public static List<Vector2Int> GetManhattanCircleAccessiblePoints(Vector2Int start, int radius, Func<Vector2Int, bool> IsPassable)
        {
            //if (!IsPassable(start))
            //{
            //    return new List<Vector2Int>();
            //}

            var accessiblePoints = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();

            queue.Enqueue(start);
            visited.Add(start);
            accessiblePoints.Add(start);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                int currentDist = Utility.Math.Abs(current.x - start.x) + Utility.Math.Abs(current.y - start.y);

                if (currentDist >= radius) continue;

                // 检查四个方向的邻居
                Vector2Int[] neighbors = 
                {
                    new Vector2Int(current.x + 1, current.y), // 右
                    new Vector2Int(current.x - 1, current.y), // 左
                    new Vector2Int(current.x, current.y + 1), // 上
                    new Vector2Int(current.x, current.y - 1)  // 下
                };

                foreach (var neighbor in neighbors)
                {
                    if (visited.Contains(neighbor)) continue;
                    visited.Add(neighbor);

                    // 检测通行性
                    if (IsPassable(neighbor))
                    {
                        accessiblePoints.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return new List<Vector2Int>(accessiblePoints);
        }

        /// <summary>
        /// 获取矩形范围
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<Vector2Int> GetRectPoints(int x, int y, int radius)
        {
            return GetRectPoints(x, y, radius, radius);
        }

        /// <summary>
        /// 获取矩形范围
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="halfWidth"></param>
        /// <param name="halfHeighth"></param>
        /// <returns></returns>
        public static List<Vector2Int> GetRectPoints(int x, int y, int halfWidth, int halfHeighth)
        {
            var points = new HashSet<Vector2Int>();

            if (halfWidth >= 0 && halfHeighth >= 0)
            {
                for (int i = -halfWidth; i <= halfWidth; i++)
                {
                    for (int j = -halfHeighth; j <= halfHeighth; j++)
                    {
                        points.Add(new Vector2Int(x + i, y + j));
                    }
                }
            }

            return points.ToList();
        }

        public static bool Contains(List<Vector2Int> points, Vector2Int point)
        {
            if (points == null || points.Count == 0) return false;
            return points.Contains(point);
        }

        public static Vector3 Coord2Pos(int coordX, int coordY)
        {
            TileItem tile = GetTile(coordX, coordY);
            if (tile == null)
            {
                MDebug.Error("tile is null : ", coordX, coordY);
                return Vector3.zero;
            }
            else
            {
                float x = tile.GetFloatValue(DataKey.PosX);
                float z = tile.GetFloatValue(DataKey.PosZ);
                return new Vector3(x, 0.05f, z);
            }
        }

        public static int Coord2Index(int x, int y)
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

        public static Vector2Int Index2Coord(int index)
        {
            int x = index % ColCount;
            int y = index / ColCount;
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// A*寻路
        /// </summary>
        /// <param name="startX">起始点x</param>
        /// <param name="startY">起始点y</param>
        /// <param name="targetX">目标点x</param>
        /// <param name="targetY">目标点y</param>
        /// <param name="H_Value">H值算法</param>
        /// <param name="IsPassable">可达性算法</param>
        /// <returns></returns>
        public static List<Vector2Int> AStarPath(int startX, int startY, int targetX, int targetY, Func<int, int, int, int, int> H_Value, Func<int, int, bool> IsPassable)
        {
            //if (!IsPassable(startX, startY) || !IsPassable(targetX, targetY))
            //{
            //    return new List<Vector2Int>();
            //}
            if (startX == targetX && startY == targetY)
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
                    int nx = x + dir.x;
                    int ny = y + dir.y;

                    if (!IsPassable(nx, ny))
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

        private static List<Vector2Int> ReconstructPath(Node node)
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

        public static TileItem GetTile(int x, int y)
        {
            TileItem item = Game.GetSystem<BattleSystem>().GridManager.GetTile(x, y);
            return item;
        }

        public static Entity GetBattleUnit(int x, int y)
        {
            var item = GetTile(x, y);
            if (item == null)
            {
                return null;
            }

            int battleId = item.GetIntValue(DataKey.BattleId);
            if (battleId <= 0)
            {
                return null;
            }

            Entity unit = Game.GetSystem<BattleSystem>().GetBattleUnit(battleId);
            return unit;
        }
        public static Entity GetBattleUnit(int index)
        {
            var coord = Index2Coord(index);
            return GetBattleUnit(coord.x, coord.y);
        }
        public static Entity GetBattleUnit(Vector2Int coord)
        {
            return GetBattleUnit(coord.x, coord.y);
        }
    }
}
