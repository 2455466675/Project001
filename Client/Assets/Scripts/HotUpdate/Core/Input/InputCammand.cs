namespace GameFramework.Core 
{
    public class InputCammand : GameCammand, IInputable
    {
        /// <summary>
        /// ÊäÈë²Ù×÷
        /// </summary>
        /// <param name="context"></param>
        public void InputAction(InputContext context)
        {
            OnInputAction(context);

            if (TryPeek(out GameCammand cammand))
            {
                if (cammand is InputCammand icammand) 
                {
                    icammand.InputAction(context);
                }
            }
        }

        protected virtual void OnInputAction(InputContext context) 
        {
            
        }
    }
}