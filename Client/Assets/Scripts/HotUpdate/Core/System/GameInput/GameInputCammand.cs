namespace GameFramework.Core
{
    public class GameInputCammand : GameCammandBase, IInputable
    {
        public void OnInput(InputContext context)
        {
            if (TryPeek(out GameInputCammand cammand))
            {
                cammand.OnInput(context);
            }
            OnInputAction(context);
        }

        protected virtual void OnInputAction(InputContext context)
        {
        }
    }
}