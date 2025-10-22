namespace GameFramework.Core 
{
    public abstract class InputCammand : GameCammand, IInputable
    {
        /// <summary>
        /// ÊäÈë²Ù×÷
        /// </summary>
        /// <param name="context"></param>
        public void InputAction(InputContext context)
        {
            if (TryPeek(out GameCammand cammand))
            {
                if (cammand is InputCammand icammand) 
                {
                    icammand.InputAction(context);
                }
            }
            OnInputAction(context);
        }

        protected virtual void OnInputAction(InputContext context) 
        {
            
        }
    }
}