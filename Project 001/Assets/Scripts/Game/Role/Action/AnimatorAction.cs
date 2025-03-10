using Sirenix.OdinInspector;
using P_Type = UnityEngine.AnimatorControllerParameterType;

namespace Game.System
{
    /// <summary>
    /// ÇÐ»»¶¯»­
    /// </summary>
	public class AnimatorAction : ActorBaseAction
    {
        public string ParameterName;

        public P_Type pt;
        [ShowIf("pt", P_Type.Int)]
        public int intValue;
        [ShowIf("pt", P_Type.Float)]
        public float floatValue;
        [ShowIf("pt", P_Type.Bool)]
        public bool boolValue;

        public override void Execute(Actor actor, params object[] actionArgs)
        {
            if (actor == null) return;

            switch (pt)
            {
                case P_Type.Float:
                    actor.animator.SetFloat(ParameterName, floatValue);
                    break;
                case P_Type.Int:
                    actor.animator.SetInteger(ParameterName, intValue);
                    break;
                case P_Type.Bool:
                    actor.animator.SetBool(ParameterName, boolValue);
                    break;
            }
        }
    }
}

