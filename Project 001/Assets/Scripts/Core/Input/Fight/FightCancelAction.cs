using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 战斗输入-取消
    /// </summary>
	public class FightCancelAction : InputActionWrapper
    {
        public FightCancelAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            MLog.Log("FightCancelAction");
        }
    }
}

