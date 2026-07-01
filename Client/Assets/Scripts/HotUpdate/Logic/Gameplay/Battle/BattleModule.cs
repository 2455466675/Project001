using Cysharp.Threading.Tasks;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Logic
{
    public class BattleFormationSite : DataModel
    {
        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private int camp;
        public int Camp
        {
            get { return camp; }
            set { SetValue(ref camp, value); }
        }

        private int state;
        public int State
        {
            get { return state; }
            set { SetValue(ref state, value); }
        }

        private bool valid;
        public bool Valid
        {
            get { return valid; }
            set {  valid = value; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set {  position = value; }
        }
    }


public class BattleFormationManager
    {
        private const string FormationCfgPath = "Assets/Bundles/Common/BattleFormationCfg";
        private const int DefaultPlayerCount = 4;
        private const int DefaultEnemyCount = 6;

        private DataModelList<BattleFormationSite> playerSites;
        private DataModelList<BattleFormationSite> enemySites;

        public DataModelList<BattleFormationSite> PlayerSites
        {
            get { return playerSites; }
        }

        public DataModelList<BattleFormationSite> EnemySites
        {
            get { return enemySites; }
        }

        public async UniTask LoadFormation(int formationID)
        {
            playerSites = new DataModelList<BattleFormationSite>();
            enemySites = new DataModelList<BattleFormationSite>();

            // 读取阵形配置：Valid 与 Position 由编辑器采集后写入资产，运行时按 id 取对应阵形
            BattleFormationCfg cfg = await Game.Assets.LoadAssetAsync<BattleFormationCfg>(FormationCfgPath);
            BattleFormationCfg.FormationData formation = cfg != null ? cfg.GetFormation(formationID) : null;

            BuildSites(playerSites, 1, formation != null ? formation.playerSites : null, DefaultPlayerCount);
            BuildSites(enemySites, 2, formation != null ? formation.enemySites : null, DefaultEnemyCount);
        }

        private void BuildSites(DataModelList<BattleFormationSite> list, int camp, System.Collections.Generic.List<BattleFormationCfg.SiteData> configSites, int fallbackCount)
        {
            bool hasConfig = configSites != null && configSites.Count > 0;
            // 有配置时以配置点位数量为准，缺配置则退回默认数量，保证战斗流程始终可用
            int count = hasConfig ? configSites.Count : fallbackCount;

            for (int i = 0; i < count; i++)
            {
                BattleFormationCfg.SiteData data = hasConfig && i < configSites.Count ? configSites[i] : null;
                BattleFormationSite site = new BattleFormationSite();
                site.Index = data != null ? data.index : i;
                site.Camp = camp; //1 = 玩家；2 = 敌人
                site.State = 0; //0 = 空；1 = 有单位；2 = 单位已死亡
                site.Valid = data != null ? data.valid : true; //位置是否可用
                site.Position = data != null ? data.position : Vector3.zero;
                list.Add(site);
            }
        }
    }

    [Gameplay]
    public class BattleModule : IGameplay
    {
        private readonly BattleFormationManager formationManager = new BattleFormationManager();
        // 当前战斗使用的阵形 id，后续可由关卡/挑战配置指定，暂以默认阵形驱动
        private int currentFormationID = 1;

        public BattleFormationManager Formation
        {
            get { return formationManager; }
        }

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
            //按阵形 id 读取点位配置，填充玩家/敌人站位数据供后续布阵使用
            await formationManager.LoadFormation(currentFormationID);
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

