using Cysharp.Threading.Tasks;
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
            EnterBattleAsync().Forget();
        }

        public void ExitBattle()
        {
            ExitBattleAsync().Forget();
        }

        private async UniTaskVoid EnterBattleAsync()
        {
            //1、战斗场景淡入动画（0.5s）
            //2、游戏状态切换
            //  Game.GetSystem<GameSceneSystem>().SetBattleSceneVisable(true);
            //  Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Combat);
            //  Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Controlled);
            //  Game.GetModule<PartyModule>().ShutDown();
            //  TODO:战斗对象加载
            //3、战斗场景淡出动画（0.5s）

            //4、开始战斗流程


            //5、普通场景淡入动画
            //6、游戏状态切换
            //7、普通场景淡出动画
                        
            await Game.GetSystem<GameTransitionManager>().Transition(Utility.GameDefine.TransitionType.EnterBattleScene, LoadBattle);            
        }

        private async UniTaskVoid ExitBattleAsync()
        {
            await Game.GetSystem<GameTransitionManager>().Transition(Utility.GameDefine.TransitionType.ExitBattleScene, UnloadBattle);
        }

        private async UniTask LoadBattle()
        {
            Game.GetModule<PartyModule>().ShutDown();
            Game.GetSystem<GameSceneSystem>().SetBattleSceneVisable(true);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Combat);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Controlled);
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = true, panel = Utility.GameDefine.PanelDefine.BattleFormation });
        }

        private async UniTask UnloadBattle()
        {
            Game.Message.SendMessage(new UIPanelMessage() { isVisible = false, panel = Utility.GameDefine.PanelDefine.BattleFormation });
            Game.GetSystem<GameSceneSystem>().SetBattleSceneVisable(false);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);
            Game.GetSystem<GameCameraController>().SetCameraModel(CameraModel.Follow);
            Game.GetModule<PartyModule>().StartUp();
        }
    }
}   

