using GameFramework.Core;

namespace GameFramework.Logic
{
    [Gameplay]
    public class BattleModule : IGameplay
    {
        void IGameplay.OnInit()
        {

        }

        void IGameplay.OnExit()
        {

        }

        public void EnterBattle()
        {
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = true, panel = Utility.GameDefine.PanelDefine.BattleLoadingPanel});
            Game.GetSystem<GameSceneSystem>().SetBattleSceneVisable(true);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Combat);
        }

        public void ExitBattle()
        {
            Game.GetSystem<GameSceneSystem>().SetBattleSceneVisable(false);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);
        }
    }
}

