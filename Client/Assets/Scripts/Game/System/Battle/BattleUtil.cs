using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem 
{
    public static class BattleUtil
    {
        public static Vector3 IndexToPosition(int x, int y) 
        {
            SceneGridView view = Game.System.BattleSystem.GetBattleSceneView<SceneGridView>();
            TileItem item = view.GetTileItem(x, y);
            if (item == null) 
            {
                return Vector3.zero;
            }
            else
            {
                return item.transform.localPosition;
            }
        }

        public static int IndexToIndex(int x, int y)
        {
            SceneGridView view = Game.System.BattleSystem.GetBattleSceneView<SceneGridView>();
            TileItem item = view.GetTileItem(x, y);
            if (item == null)
            {
                return -1;
            }
            else
            {
                return item.Index;
            }
        }

        public static List<Vector2Int> SimplifyPath(List<Vector2Int> path) 
        {
            List<Vector2Int> result = new List<Vector2Int>();
            if (path == null || path.Count < 2) 
            {
                return result;
            }

            var p0 = path[0];
            var p1 = path[1];
            bool isH = p0.y == p1.y;

            result.Add(p0);

            var ep = p1;

            if (path.Count == 2)
            {
                result.Add(p1);
            }
            else
            {
                for (int i = 2; i < path.Count; i++)
                {
                    var p = path[i];

                    if (isH)
                    {
                        if (p.y != ep.y)
                        {
                            result.Add(path[i - 1]);
                            ep = p;
                            isH = false;
                        }
                    }
                    else
                    {
                        if (p.x != ep.x)
                        {
                            result.Add(path[i - 1]);
                            ep = p;
                            isH = true;
                        }
                    }

                    if (i == path.Count - 1)
                    {
                        result.Add(p);
                    }
                }
            }

            return result;
        }
    }
}