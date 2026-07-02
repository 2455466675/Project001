using Cysharp.Threading.Tasks;
using GameFramework.Core;

namespace GameFramework.Logic
{
    public enum BattleCamp
    {
        None = 0,
        Player = 1,
        Enemy = 2,
    }

    [Gameplay]
    public class BattleModule : IGameplay
    {
        private int currentFormationID = 6;

        public BattleFormation Formation { get; private set; }
        public BattleUnit Unit { get; private set; }

        void IGameplay.OnInit()
        {
            Formation = new BattleFormation();
            Unit = new BattleUnit();
        }

        void IGameplay.OnExit()
        {

        }

        public void EnterBattle()
        {
            EnterBattleAsync().Forget();
        }

        public void ExitBattle()
        {
            ExitBattleAsync().Forget();
        }

        private async UniTaskVoid EnterBattleAsync()
        {        
            await Game.GetSystem<GameTransitionManager>().Transition(Utility.GameDefine.TransitionType.EnterBattleScene, LoadBattle);            
        }

        private async UniTaskVoid ExitBattleAsync()
        {
            await Game.GetSystem<GameTransitionManager>().Transition(Utility.GameDefine.TransitionType.ExitBattleScene, UnloadBattle);
        }

        private async UniTask LoadBattle()
        {
            Game.GetModule<PartyModule>().ShutDown();
            
            await Game.GetSystem<GameSceneSystem>().LoadScene(1003);
            
            await Formation.LoadFormation(currentFormationID);

            Unit.CreateUnit(null);

            await Unit.RefreshUnit();

            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Combat);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Controlled);
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = true, panel = Utility.GameDefine.PanelDefine.BattleFormation });
        }

        private async UniTask UnloadBattle()
        {
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = false, panel = Utility.GameDefine.PanelDefine.BattleFormation });
            //卸载战斗场景
            await Game.GetSystem<GameSceneSystem>().UnloadScene(1003);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Follow);
            Game.GetModule<PartyModule>().StartUp();
        }
    }
}   

