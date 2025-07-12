using Sirenix.OdinInspector;
using UnityEngine;
using PT = UnityEngine.AnimatorControllerParameterType;

namespace Game.GSystem
{
    public enum AnimatorActionType 
    {
        ParameterFloat = PT.Float,
        ParameterInt = PT.Int,
        ParameterBool = PT.Bool,
        ParameterTrigger = PT.Trigger,
        Play = 99,
    }

    public class AnimatorCommand : ActionCommand<AnimatorCommandExecutor>
    {
        public string parameter;
        public AnimatorActionType animationType = AnimatorActionType.ParameterFloat;

        [ShowIf("animationType", AnimatorActionType.ParameterInt)]
        public int intValue;

        [ShowIf("animationType", AnimatorActionType.ParameterFloat)]
        public float floatValue;

        [ShowIf("animationType", AnimatorActionType.ParameterBool)]
        public bool boolValue;

        [ShowIf("animationType", AnimatorActionType.Play)]
        public int layer;

        public int ParameterHash
        {
            get 
            {
                return Animator.StringToHash(parameter);
            }
        }
    }

    public class AnimatorCommandExecutor : ActionCommandExecutor<AnimatorCommand>
    {
        private bool isLoop;
        private int lastState;

        protected override void OnExecute()
        {
            Actor actor = Player.Actor;
            if (actor == null) 
            {
                return;
            }

            Animator animator = actor.animator;
            if (animator == null)
            {
                return;
            }

            int hash = Command.ParameterHash;

            AnimatorActionType at = Command.animationType;
            switch (at)
            {
                case AnimatorActionType.ParameterFloat:
                    animator.SetFloat(hash, Command.floatValue);
                    break;
                case AnimatorActionType.ParameterInt:
                    animator.SetInteger(hash, Command.intValue);
                    break;
                case AnimatorActionType.ParameterBool:
                    animator.SetBool(hash, Command.boolValue);
                    break;
                case AnimatorActionType.ParameterTrigger:
                    animator.SetTrigger(hash);                    
                    break;
                case AnimatorActionType.Play:
                    AnimatorStateInfo last = animator.GetCurrentAnimatorStateInfo(0);
                    animator.Play(hash, Command.layer);
                    AnimatorStateInfo curr = animator.GetCurrentAnimatorStateInfo(0);
                    if (curr.loop) 
                    {
                        isLoop = true;                   
                        lastState = last.shortNameHash;
                    }
                    else
                    {
                        isLoop = false;
                    }
                    break;
                default:
                    Debug.LogError("无效的动画参数类型");
                    break;
            }
        }

        protected override void OnComplete()
        {
            Actor actor = Player.Actor;
            if (actor == null)
            {
                return;
            }

            Animator animator = actor.animator;
            if (animator == null)
            {
                return;
            }

            AnimatorActionType at = Command.animationType;
            if (at != AnimatorActionType.Play) 
            {
                return;
            }

            if (!isLoop) 
            {
                return;
            }

            animator.Play(lastState, Command.layer);
        }
    }
}