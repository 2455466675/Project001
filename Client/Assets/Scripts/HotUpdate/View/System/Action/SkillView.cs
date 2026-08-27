using Cysharp.Threading.Tasks;
using GameFramework.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.View.Action
{
    public class EffectResult //需要和Logic.EffectResult统一
    {
        public int Seq;
        public int SegmentIndex;
        public int Source;
        public int Target;
        public string Outcome;
        public int Value;
        public int SkillId;
    }

    public class SkillActionReport //战斗产出
    {
        public int SkillId;
        public int Caster;
        public List<int> Targets;
        public List<EffectResult> Results;
    }

    public class SkillViewContext
    {
        private readonly UniTaskCompletionSource _hitSource = new UniTaskCompletionSource();

        public UniTask WaitForHit => _hitSource.Task;

        public void SetHit()
        {
            _hitSource.TrySetResult();
        }
    }

    [Serializable]
    public abstract class SkillAction
    {
        public abstract UniTask Invoke(SkillViewContext context, IActor actor);
    }

    public class PresentationAction : SkillAction
    {
        [AbstractType(typeof(BaseAction))]
        [SerializeReference]
        public BaseAction action;

        public override UniTask Invoke(SkillViewContext context, IActor actor)
        {
            if (action == null)
            {
                return UniTask.CompletedTask;
            }
            else
            {
                return action.Invoke(actor);
            }
        }
    }

    public class HitPointAction : SkillAction
    {        
        public override UniTask Invoke(SkillViewContext context, IActor actor)
        {
            context.SetHit();
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class SkillActionTrack
    {
        public enum PlayModel
        {
            Parallel,
            Sequence,
        }

        public PlayModel playModel;

        [AbstractType(typeof(SkillAction))]
        [SerializeReference]
        public SkillAction[] actions;

        public async UniTask Play(SkillViewContext context, IActor actor)
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
                        tasks.Add(actions[i].Invoke(context, actor));
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
                        await actions[i].Invoke(context, actor);
                    }
                }
            }
        }
    }

    [CreateAssetMenu(fileName = "SkillView", menuName = "Scriptable Objects/SkillView")]
    public class SkillView : ScriptableObject
    {
        [SerializeField]
        private SkillActionTrack[] CasterTrack;
        [SerializeField]
        private SkillActionTrack[] TargetsTrack;

        public async UniTask Play(SkillActionReport report)
        {
            SkillViewContext context = new SkillViewContext();

            var task1 = PlayCasterTrack(context, report);
            var task2 = PlayTargetsTrack(context, report);

            var tracksTask = UniTask.WhenAll(task1, task2).ContinueWith(() => context.SetHit());

            var task3 = ResolveEffectResult(context, report);
            await UniTask.WhenAll(tracksTask, task3);
        }

        private async UniTask PlayCasterTrack(SkillViewContext context, SkillActionReport report)
        {
            var caster = report.Caster;
            IActor casterActor = null;
            List<UniTask> tasks = new List<UniTask>();
            foreach (var track in CasterTrack)
            {
                tasks.Add(track.Play(context, casterActor));
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask PlayTargetsTrack(SkillViewContext context, SkillActionReport report)
        {
            List<UniTask> tasks = new List<UniTask>();
            var targets = report.Targets;
            foreach (var target in targets)
            {
                IActor targetActor = null;
                List<UniTask> tasks2 = new List<UniTask>();
                foreach (var track in TargetsTrack)
                {
                    tasks2.Add(track.Play(context, targetActor));
                }
                tasks.Add(UniTask.WhenAll(tasks2));
            }
            await UniTask.WhenAll(tasks);
        }

        private async UniTask ResolveEffectResult(SkillViewContext context, SkillActionReport report)
        {
            await context.WaitForHit;

            List<int> segmentIndexs = new List<int>();
            Dictionary<int, Dictionary<int, List<EffectResult>>> temp = new Dictionary<int, Dictionary<int, List<EffectResult>>>();
            List<EffectResult> results = report.Results;
            foreach (var item in results)
            {
                int segmentIndex = item.SegmentIndex;
                if (!temp.TryGetValue(segmentIndex, out var group))
                {
                    group = new Dictionary<int, List<EffectResult>>();
                    temp[segmentIndex] = group;
                    segmentIndexs.Add(segmentIndex);
                }

                int targetId = item.Target;
                if (!group.TryGetValue(targetId, out var effectList))
                {
                    effectList = new List<EffectResult>();
                    group[targetId] = effectList;
                }
                effectList.Add(item);
            }

            segmentIndexs.Sort();

            foreach (var segmentIndex in segmentIndexs)
            {
                var group = temp[segmentIndex];
                List<UniTask> tasks = new List<UniTask>();
                foreach (var item in group)
                {
                    tasks.Add(PlayEffect(item.Key, item.Value));
                }
                await UniTask.WhenAll(tasks);
            }
        }

        private async UniTask PlayEffect(int id, List<EffectResult> effects)
        {
            int targetId = id;
            IActor targetActor = null; // TODO targetId => targetActor

            effects.Sort((a, b) => a.Seq.CompareTo(b.Seq));

            foreach (var effectReslut in effects)
            {
                var outcome = effectReslut.Outcome;
                ActionView actionView = null; // TODO outcome => actionView
                await actionView.Play(targetActor);
            }
        }
    }
}
