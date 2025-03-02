using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 战斗输入-确定
    /// </summary>
	public class FightSubmitAction : InputActionWrapper
    {
        public FightSubmitAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            MLog.Log("FightSubmitAction");
        }
    }
}

