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
            var type = arg.type;
            int index = BattleUtils.Coord2Index(x, y);
            Game.GetSystem<BattleViewSystem>().FocusBattleGrid(index, type);
        }
    }
}
