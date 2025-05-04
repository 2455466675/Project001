using Sirenix.OdinInspector;
using UnityEngine;
using PT = UnityEngine.AnimatorControllerParameterType;

namespace Game.System
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

        private int parameterHash = -1;
        public int ParameterHash
        {
            get 
            {
                if (parameterHash < 0) 
                {
                    parameterHash = Animator.StringToHash(parameter);
                }
                return parameterHash;
            }
        }
    }

    public class AnimatorActionCommand : ActionCommand<AnimatorAction>
    {
        protected override void OnExecute()
        {
            Actor actor = Player.Actor;
            if (actor == null || actor.animator == null) 
            {
                return;
            }

            int hash = Item.ParameterHash;
            if (hash < 0) 
            {
                return;
            }

            AnimatorActionType at = Item.animationType;
            switch (at)
            {
                case AnimatorActionType.ParameterFloat:
                    actor.animator.SetFloat(hash, Item.floatValue);
                    break;
                case AnimatorActionType.ParameterInt:
                    actor.animator.SetInteger(hash, Item.intValue);
                    break;
                case AnimatorActionType.ParameterBool:
                    actor.animator.SetBool(hash, Item.boolValue);
                    break;
                case AnimatorActionType.ParameterTrigger:
                    actor.animator.SetTrigger(hash);                    
                    break;
                case AnimatorActionType.Play:
                    actor.animator.Play(hash, Item.layer);
                    break;
                default:
                    Debug.LogError("无效的动画参数类型");
                    break;
            }
        }
    }
}