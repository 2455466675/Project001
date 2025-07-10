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
    }
}