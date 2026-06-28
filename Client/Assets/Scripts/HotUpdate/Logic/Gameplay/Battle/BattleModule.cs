using Cysharp.Threading.Tasks;
using GameFramework.Core;

namespace GameFramework.Logic
{
    [Gameplay]
    public class BattleModule : IGameplay
    {
        //战斗对象
        //阵形

        void IGameplay.OnInit()
        {

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
            //加载战斗场景
            await Game.GetSystem<GameSceneSystem>().LoadScene(1003);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Combat);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Controlled);
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = true, panel = Utility.GameDefine.PanelDefine.BattleFormation });
            await UniTask.CompletedTask;
        }

        private async UniTask UnloadBattle()
        {
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = false, panel = Utility.GameDefine.PanelDefine.BattleFormation });
            //卸载战斗场景
            await Game.GetSystem<GameSceneSystem>().UnloadScene(1003);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Follow);
            Game.GetModule<PartyModule>().StartUp();
            await UniTask.CompletedTask;
        }
    }
}   

