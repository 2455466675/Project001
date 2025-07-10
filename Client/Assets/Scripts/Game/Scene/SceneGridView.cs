using Codice.Client.BaseCommands;
using Game.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game 
{
    public class SceneGridView : View
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

        [SerializeField]
        private int row;
        [SerializeField]
        private int col;
        [SerializeField]
        private float w;
        [SerializeField]
        private float h;

        [SerializeField]
        private TileItem tileItem;

        [SerializeField]
        private TileItem[] items;

        public void ResetAllItem() 
        {
            if (items == null || items.Length == 0) 
            {
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetState(TitleState.Normal);
            }
        }

        public TileItem GetTileItem(int x, int y) 
        {
            int index = (x * row) + y;
            if (index < 0 || index >= items.Length)
            {
                return null;
            }
            return items[index];
        }

        public List<Vector2Int> ReachableRange(int startX, int startY, int step) 
        {
            List<Vector2Int> result = new List<Vector2Int>();
            if (!CheckIndexValid(startX, startY))
            {
                return result;
            }

            if (step <= 0) 
            {
                return result;
            }

            bool[,] closeList = new bool[col, row];
            Queue<Node> openList = new Queue<Node>();

            Node startNode = new Node(startX, startY, 0, 0, null);
            openList.Enqueue(startNode);

            for (int i = 0; i < step; i++)
            {
                Queue<Node> temp = new Queue<Node>();
                while (openList.Count > 0)
                {
                    Node curr = openList.Dequeue();
                    int x = curr.X;
                    int y = curr.Y;
                    if (closeList[x, y])
                    {
                        continue;
                    }
                    else
                    {
                        closeList[x, y] = true;
                    }

                    for (int j = 0; j < neighbor.Count; j++)
                    {
                        var dir = neighbor[j];
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

                        Node neighborNode = new Node(nx, ny, 0, 0, null);
                        temp.Enqueue(neighborNode);
                        result.Add(new Vector2Int(nx, ny));
                    }
                }
                openList = temp;
            }

            return result;
        }

        public List<Vector2Int> AStarPath(int startX, int startY, int targetX, int targetY) 
        {
            if (!CheckIndexValid(startX, startY) || !CheckIndexValid(targetX, targetY)) 
            {
                return new List<Vector2Int>();
            }

            bool[,] closeList = new bool[col, row];
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
            if (x < 0 || x >= col || y < 0 || y >= row) 
            {
                return false;
            }

            TileItem item = GetTileItem(x, y);
            if (item == null) 
            {
                return false;
            }

            return item.Passable(MoveType.Walk);         
        }

        private int H_Value(int startX, int startY, int targetX, int targetY) 
        {
            return GameMathf.Abs(startX - targetX) + Math.Abs(startY - targetY);
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

        [Button("Init")]
        private void Init() 
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) 
            {                
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            items = new TileItem[col * row];

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    TileItem item = Instantiate(tileItem, transform);

                    float x = w * i;
                    float z = h * j;

                    item.transform.localPosition = new Vector3(x, 0, z);
                    item.SetIndex(i, j);
                    items[(i * row) + j] = item;
                }
            }
        }
    }
}