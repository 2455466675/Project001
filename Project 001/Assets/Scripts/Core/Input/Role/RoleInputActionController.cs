using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 场景角色行为控制器
    /// </summary>
	public class RoleInputActionController : InputActionController
    {
        public override InputMode Mode => InputMode.Role;
        public RoleInputActionController(MyInput inputActions) : base(inputActions)
        {
            actions = new List<InputActionWrapper>
            {
                new RoleMoveAction(inputActions.Role.Move),
                new RoleMenuAction(inputActions.Role.Menu),
                new RoleAddSpeedAction(inputActions.Role.AddSpeed)
            };
        }
        public override void Enable()
        {
            inputActions.Role.Enable();
        }

        public override void Disable()
        {
            inputActions.Role.Disable();
        }
    }
}

