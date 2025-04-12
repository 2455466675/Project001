using Sirenix.OdinInspector;
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

            string parameter = Item.parameter;
            if (string.IsNullOrEmpty(parameter)) 
            {
                return;
            }

            PT pt = Item.parameterType;
            switch (pt)
            {
                case PT.Float:
                    actor.animator.SetFloat(parameter, Item.floatValue);
                    break;
                case PT.Int:
                    actor.animator.SetInteger(parameter, Item.intValue);
                    break;
                case PT.Bool:
                    actor.animator.SetBool(parameter, Item.boolValue);
                    break;
                case PT.Trigger:
                    actor.animator.SetTrigger(parameter);
                    break;
                default:
                    break;
            }
        }
    }
}