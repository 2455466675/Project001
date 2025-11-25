using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleTransformComponent : Featrue.Component
    {
        private int coordX;
        private int coordY;
        public int CoordX => coordX;
        public int CoordY => coordY;

        public void SetCoordPosition(int coordX, int coordY)
        {
            this.coordX = coordX;
            this.coordY = coordY;

            Vector3 pos = Game.GetSystem<BattleSystem>().GridManager.Coord2Pos(coordX, coordY);
            GetComponent<ActorComponent>().Position = pos;
        }
    }
}
