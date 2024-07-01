using P_Type = UnityEngine.AnimatorControllerParameterType;

namespace Game.Core
{
    /// <summary>
    /// ÇÐ»»¶¯»­
    /// </summary>
	public class AnimatorAction : BaseAction
    {
        public string ParameterName;
        public string value;

        public P_Type pt;
        private int intValue;
        private float floatValue;
        private bool boolValue;

        private void Start()
        {
            switch (pt)
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

        public override void Exit()
        {
            
        }
    }
}

