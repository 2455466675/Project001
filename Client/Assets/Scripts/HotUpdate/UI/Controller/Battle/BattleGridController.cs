using Cysharp.Threading.Tasks;
using GameFramework.Core;
using GameFramework.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI
{
    public class BattleGridController : NavigationController
    {
        protected override void OnShow()
        {           
            var tiles = Game.GetSystem<BattleSystem>().GridManager.GetTiles();
            SetData(tiles);
        }

        protected override void BindItemView(NavigationItemView itemView, DataModel dataModel)
        {
            Vector3 pos = itemView.transform.position;
            dataModel.SetValue("posX", pos.x);
            dataModel.SetValue("posZ", pos.z);
        }

        protected override void RefreshItemView(NavigationItemView itemView, IReadOnlyDataModel dataModel)
        {
            var widget = itemView.GetWidget<TileStateWidget>();
            if (widget != null)
            {
                int state = dataModel.GetIntValue("state");
                widget.SetState(state);
            }
        }

        protected override void SelectItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue("index");
            Game.GetSystem<BattleSystem>().GridManager.SelectTile(index);
        }

        protected override void DeselectItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue("index");
            Game.GetSystem<BattleSystem>().GridManager.DeselectTile(index);
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue("index");

            var bs = Game.GetSystem<BattleSystem>();
            var e = bs.Entity;
            var bfc = e.GetComponent<BattleTransformComponent>();
            var bmc = e.GetComponent<BattleMotorComponent>();

            var coord = bs.GridManager.Index2Coord(index);
            var path = bs.GridManager.AStarPath(bfc.CoordX, bfc.CoordY, coord.x, coord.y);

            bmc.MoveAsync(path).Forget();

            Game.GetModule<CameraManager>().LookAt(bs.GridManager.Coord2Pos(coord.x, coord.y));
        }
    }
}
