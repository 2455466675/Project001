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
            Game.GetSystem<BattleViewSystem>().SelectBattleTile(index);
        }

        protected override void DeselectItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().DeselectBattleTile(index);
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().SubmitBattleTile(index);
        }

        protected override void MoveUpItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().MoveUp(index);
        }

        protected override void MoveDownItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().MoveDown(index);
        }

        protected override void MoveLeftItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().MoveLeft(index);
        }

        protected override void MoveRightItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int index = dataModel.GetIntValue(DataKey.Index);
            Game.GetSystem<BattleViewSystem>().MoveRight(index);
        }
    }
}
