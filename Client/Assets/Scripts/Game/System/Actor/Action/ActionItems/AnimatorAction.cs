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

    public class AnimatorAction : ActionItem<AnimatorActionCommand>
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

    public class AnimatorActionCommand : ActionCommand<AnimatorAction>
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

            int hash = Item.ParameterHash;

            AnimatorActionType at = Item.animationType;
            switch (at)
            {
                case AnimatorActionType.ParameterFloat:
                    animator.SetFloat(hash, Item.floatValue);
                    break;
                case AnimatorActionType.ParameterInt:
                    animator.SetInteger(hash, Item.intValue);
                    break;
                case AnimatorActionType.ParameterBool:
                    animator.SetBool(hash, Item.boolValue);
                    break;
                case AnimatorActionType.ParameterTrigger:
                    animator.SetTrigger(hash);                    
                    break;
                case AnimatorActionType.Play:
                    AnimatorStateInfo last = animator.GetCurrentAnimatorStateInfo(0);
                    animator.Play(hash, Item.layer);
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

            AnimatorActionType at = Item.animationType;
            if (at != AnimatorActionType.Play) 
            {
                return;
            }

            if (!isLoop) 
            {
                return;
            }

            animator.Play(lastState, Item.layer);
        }
    }
}