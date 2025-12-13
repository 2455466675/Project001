using GameFramework.Core;

namespace GameFramework.UI
{
    public class PlayAnimActionCommand : ActionCommand
    {
        public string animName;

        protected override void OnExecute()
        {

            if (string.IsNullOrEmpty(animName))
            {
                return;
            }

            if (actionData == null || actionData.actor == null)
            {
                return;
            }

            actionData.actor.PlayAnim(animName);
        }
    }
}
