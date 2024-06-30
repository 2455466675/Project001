using P = UnityEngine.AnimatorControllerParameter;
using P_Type = UnityEngine.AnimatorControllerParameterType;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class AnimatorAction : BaseAction
    {
        public string ParameterName;
        public string value;

        private P_Type pType;
        private int intValue;
        private float floatValue;
        private bool boolValue;

        private void Start()
        {
            var p = FindParameter(ParameterName);
            if (p == null)
            {
                return;
            }
            pType = p.type;
            switch (pType)
            {
                case P_Type.Int:
                    int.TryParse(value, out intValue);
                    break;
                case P_Type.Float:
                    float.TryParse(value, out floatValue);                    
                    break;
                case P_Type.Bool:
                    bool.TryParse(value, out boolValue);
                    break;
            }
        }

        public override void Execute()
        {            
            switch (pType)
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

        public override void Exit()
        {
            
        }

        public P FindParameter(string name)
        {
            foreach (var item in actor.animator.parameters)
            {
                if (item.name == name)
                    return item;
            }
            
            return null;
        }
    }
}

