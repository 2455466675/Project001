namespace GameFramework.Core 
{
    public class InputCommand : GameCommand, IInputable
    {
        /// <summary>
        /// ÊäÈë²Ù×÷
        /// </summary>
        /// <param name="context"></param>
        public void InputAction(InputContext context)
        {
            OnInputAction(context);

            if (TryPeek(out GameCommand command))
            {
                if (command is InputCommand icommand) 
                {
                    icommand.InputAction(context);
                }
            }
        }

        protected virtual void OnInputAction(InputContext context) 
        {
            
        }
    }
}