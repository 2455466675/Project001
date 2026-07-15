using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using GameFramework.Logic;

namespace GameFramework.View.Action
{
    [Serializable]
    public class ActionTrack
    {
        public enum PlayModel
        {
            Parallel,
            Sequence,
        }

        public PlayModel playModel;

        [AbstractType(typeof(BaseAction))]
        [SerializeReference]
        public BaseAction[] actions;

        public async UniTask Play(IActor actor)
        {
            if (actions == null || actions.Length == 0)
            {
                return;
            }

            if (playModel == PlayModel.Parallel)
            {
                List<UniTask> tasks = new List<UniTask>(actions.Length);
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                    {
                        tasks.Add(actions[i].Invoke(actor));
                    }                    
                }
                await UniTask.WhenAll(tasks);
            }

            if (playModel == PlayModel.Sequence)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                    {
                        await actions[i].Invoke(actor);
                    }                    
                }
            }
        }
    }

    [CreateAssetMenu(fileName = "ActionView", menuName = "Scriptable Objects/ActionView")]
    public class ActionView : ScriptableObject
    {
        [SerializeField]
        private ActionTrack[] tracks;

        public ActionHandle Play(IActor actor)
        {
            if (tracks == null || tracks.Length == 0)
            {
                return ActionHandle.Completed;
            }

            List<UniTask> tasks = new List<UniTask>(tracks.Length);
            for (int i = 0; i < tracks.Length; i++)
            {
                tasks.Add(tracks[i].Play(actor));
            }
            var task = UniTask.WhenAll(tasks);
            return new ActionHandle(task);
        }
    }
}
