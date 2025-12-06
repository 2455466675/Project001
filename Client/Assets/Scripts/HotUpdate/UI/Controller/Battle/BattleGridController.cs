using GameFramework.Gameplay;
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
            dataModel.SetValue(DataKey.PosX, pos.x);
            dataModel.SetValue(DataKey.PosZ, pos.z);
        }

        protected override void RefreshItemView(NavigationItemView itemView, IReadOnlyDataModel dataModel)
        {
            var widget = itemView.GetWidget<TileStateWidget>();
            if (widget != null)
            {
                int state = dataModel.GetIntValue(DataKey.State);
                widget.SetState(state);
            }
        }

        protected override void SelectItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleSystem>().GridManager.SelectTile(index);
        }

        protected override void DeselectItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleSystem>().GridManager.DeselectTile(index);
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);

            DataModel param = new DataModel();
            param.SetValue(DataKey.Index, index);

            Game.GetSystem<BattleSystem>().FlowManager.MoveNext(new MoveToTargetPosition(), param);
        }
    }
}
