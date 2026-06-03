using Cysharp.Threading.Tasks;
using GameFramework.Utility.GameDefine;
using System;
using UnityEngine;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameTransitionManager : IGameSystem, IAsyncInit
    {
        private GameTransitionConfig config;
        private bool isDone;

        async UniTask IAsyncInit.Init()
        {
            config = await Game.Resources.LoadAssetAsync<GameTransitionConfig>("Assets/Bundles/Common/GameTransitionConfig");
        }

        public async UniTask Transition(TransitionType transitionType, Func<UniTask> parallelTask)
        {
            int fadeInTimeMS = config.GetFadeInTime(transitionType);
            int fadeOutTimeMS = config.GetFadeOutTime(transitionType);
            int transitionTimeMS = config.GetTransitionTime(transitionType);
            float transitionTime = transitionTimeMS / 1000f;
            Game.Message.SendMessage(new GameTransitionFadeInMessage() { transitionType = transitionType, fadeInTime = fadeInTimeMS / 1000f });

            if (fadeInTimeMS > 0)
            {
                await UniTask.Delay(fadeInTimeMS);
            }
            else
            {
                await UniTask.CompletedTask;
            }

            isDone = false;
            UniTask handler = WrapTask(parallelTask);
            float progress = 0f;
            float t = 0f;
            while (!isDone)
            {
                if (t < transitionTime)
                {
                    t += Time.deltaTime;
                }
                progress = Utility.Util.Math.Min(0.9f, t / transitionTime * 0.9f);
                Game.Message.SendMessage(new GameTransitionProgressMessage() { transitionType = transitionType, progress = progress });
                await UniTask.Yield();
            }

            await handler;

            float progress2 = progress;
            float remain = transitionTime - t;
            float t2 = 0f;
            while (t2 < remain)
            {
                t2 += Time.deltaTime;
                progress2 = progress + t2 / remain * (1f - progress);
                Game.Message.SendMessage(new GameTransitionProgressMessage() { transitionType = transitionType, progress = progress2 });
                await UniTask.Yield();
            }

            Game.Message.SendMessage(new GameTransitionFadeOutMessage() { transitionType = transitionType, fadeOutTime = fadeOutTimeMS / 1000f });
            if (fadeOutTimeMS > 0)
            {
                await UniTask.Delay(fadeOutTimeMS);
            }
            else
            {
                await UniTask.CompletedTask;
            }
        }

        private async UniTask WrapTask(Func<UniTask> task)
        {
            try
            {
                if (task == null)
                {
                    await UniTask.CompletedTask;
                }
                else
                {
                    await task.Invoke();
                }                
            }
            finally
            {
                isDone = true;
            }
        }
    }
}
