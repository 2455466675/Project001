namespace GameFramework.Core
{
    public class GameInputCommand : GameCommandBase, IInputable
    {
        public void OnInput(InputContext context)
        {
            if (TryPeek(out GameInputCommand command))
            {
                command.OnInput(context);
            }
            OnInputAction(context);
        }

        protected virtual void OnInputAction(InputContext context)
        {
        }
    }
}
