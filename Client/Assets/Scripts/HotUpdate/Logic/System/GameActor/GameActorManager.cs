using Config;
using UnityEngine;
using GameFramework.Core;
using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    /// <summary>
    /// Actor 加载/释放的中间层。
    /// 只负责屏蔽配置查询与资源实例化细节，本身不持有 Actor 引用，
    /// 生命周期归属由调用方（ActorComponent）掌控，避免管理器与业务状态耦合。
    /// </summary>
    [GameSystem]
    public class GameActorManager : IGameSystem
    {
        public async UniTask<Actor> LoadActorAsync(int id)
        {
            ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
            if (cfg == null)
            {
                MDebug.Error("ActorCfg is null : ", id);
                return null;
            }

            GameObject obj = await Game.Assets.InstantiateAsync(cfg.PrefabPath, null);
            if (obj == null)
            {
                return null;
            }

            Actor actor = obj.GetComponent<Actor>();
            actor.Id = id;
            return actor;
        }

        public void ReleaseActor(Actor actor)
        {
            if (actor == null)
            {
                return;
            }
            Game.Assets.ReleaseAsset(actor.gameObject);
        }
    }
}
