using GameFramework.Gameplay;

namespace GameFramework.UI
{
    [GameEvent]
    public class NavigateBattleGrid_Handler : GameEventHandlerBase<NavigateBattleGridArgs>
    {
        public override void Invoke(NavigateBattleGridArgs arg)
        {
            int x = arg.coordX;
            int y = arg.coordY;
            int index = Game.GetSystem<BattleSystem>().GridManager.Coord2Index(x, y);
            Game.GetSystem<BattleViewSystem>().SelectGrid(index);
        }
    }
}
