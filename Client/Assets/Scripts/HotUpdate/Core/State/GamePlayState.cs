using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Core
{
    public class GamePlayState : StateItemBase
    {
        private static float beginProgress = 0f;

        private class Play2LoginTrigger : StateTriggerBase<GameLoginState>
        {
            public override bool Check(IBlackboard blackboard)
            {
                int v = blackboard.GetBlackboardIntValue(StateManager.PhaseKey);
                return v == (int)StateManager.GamePhase.Login;
            }
        }

        protected override void OnEnter()
        {
            LoadGame().Forget();
        }

        protected override void OnExit()
        {
            Game.Exit();
        }

        private async UniTaskVoid LoadGame()
        {
            Game.Event.Publish(new UIPanelEventArgs() { panelDefine = PanelDefine.LoadingPanel, isShow = true });

            beginProgress = 0f;
            await SetProgress(0f);

            SaveManager saveManager = Game.GetModule<SaveManager>();
            SceneManager sceneManager = Game.GetModule<SceneManager>();

            int saveIndex = GetBlackboardIntValue("SaveIndex");

            int sceneId = saveManager.GetSaveSceneId(saveIndex);
            await sceneManager.LoadScene(sceneId);

            await sceneManager.LoadBattleScene();

            await SetProgress(0.5f);

            saveManager.Load(saveIndex);

            await SetProgress(1f);
            Game.GetModule<CameraManager>().SetState(CameraState.Follow);
            Game.Event.Publish(new UIPanelEventArgs() { panelDefine = PanelDefine.LoadingPanel, isShow = false });
        }

        private async UniTask SetProgress(float progress) 
        {
            while (progress > beginProgress)
            {
                beginProgress += Time.deltaTime;
                Game.Event.Publish(new LoadingProgressEventArgs() { progress = beginProgress });
                await UniTask.Yield();
            }
        }
    }
}