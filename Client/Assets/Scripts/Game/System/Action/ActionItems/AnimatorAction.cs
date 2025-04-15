using Sirenix.OdinInspector;
using UnityEngine;
using PT = UnityEngine.AnimatorControllerParameterType;

namespace Game.System
{
    public class AnimatorAction : ActionItem<AnimatorActionCommand>
    {
        public string parameter;
        public PT parameterType = PT.Float;

        [ShowIf("parameterType", PT.Int)]
        public int intValue;

        [ShowIf("parameterType", PT.Float)]
        public float floatValue;

        [ShowIf("parameterType", PT.Bool)]
        public bool boolValue;

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

            PT pt = Item.parameterType;
            switch (pt)
            {
                case PT.Float:
                    actor.animator.SetFloat(hash, Item.floatValue);
                    break;
                case PT.Int:
                    actor.animator.SetInteger(hash, Item.intValue);
                    break;
                case PT.Bool:
                    actor.animator.SetBool(hash, Item.boolValue);
                    break;
                case PT.Trigger:
                    actor.animator.SetTrigger(hash);
                    break;
                default:
                    break;
            }
        }
    }
}