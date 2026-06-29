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
    
            /*
            *加载阵形（角色配置、位置）
            *创建对象
            
            流程：
            1、入场阶段（对象入场，播放入场动画）                
            2、回合循环 (Loop)
            {
                单回合阶段：
                {
                    1、回合开始阶段
                    2、回合进行阶段：
                    {   
                        1、确定行动对象
                        2、等待对象行动：
                        {
                            1、行动前事件
                            2、选择行为（玩家输入 or AI）
                            3、等待行为结算
                            4、行动结束事件
                        }
                        3、下一个对象行动（行动队列为空，回合结束）                        
                    }
                    3、回合结束阶段
                }
            }
            3、结算阶段
            */
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

