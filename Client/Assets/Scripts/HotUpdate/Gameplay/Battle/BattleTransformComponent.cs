using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleTransformComponent : TransformComponent
    {
        private int coordX;
        private int coordY;

        public int CoordX => coordX;
        public int CoordY => coordY;     

        protected override void OnInit()
        {
            coordX = -1;
            coordY = -1;
        }

        public void SetCoordPosition(int coordX, int coordY)
        {
            if (this.coordX >= 0 && this.coordY >= 0)
            {
                var tileItem = BattleUtils.GetTile(this.coordX, this.coordY);
                if (tileItem != null)
                {
                    tileItem.SetValue(DataKey.BattleId, 0);
                }
                else
                {
                    MDebug.Error("tileItem is null : ", this.coordX, this.coordY);
                }
            }

            this.coordX = coordX;
            this.coordY = coordY;

            if (this.coordX >= 0 && this.coordY >= 0)
            {
                var tileItem = BattleUtils.GetTile(this.coordX, this.coordY);
                if (tileItem != null)
                {
                    tileItem.SetValue(DataKey.BattleId, GetComponent<BattleUnitComponent>().BattleId);
                }
                else
                {
                    MDebug.Error("tileItem is null : ", this.coordX, this.coordY);
                }
            }

            SyncActorPosition();
        }

        public void SyncActorPosition()
        {
            GetComponent<ActorComponent>().Position = BattleUtils.Coord2Pos(coordX, coordY);
        }
    }
}
