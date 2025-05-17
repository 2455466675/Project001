using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Game.State
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : StateBase
    {
        public override GameStateDefine Define => GameStateDefine.Playing;

        public override void Enter()
        {
            Game.UI.Close();
        
            List<UniTask> tasks = new List<UniTask>();
            tasks.Add(Game.Scene.PreloadBattleScene());

            Game.Scene.LoadSceneAsync(10003, OnStartLoad, OnEndLoad, tasks).Forget();
        }

        public override void Exit()
        {
            
        }

        private void OnStartLoad() 
        {
            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Loading_Group);
            e?.Show();
        }

        private void OnEndLoad()
        {
            var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Loading_Group);
            e?.Hide();
        }
    }
}
