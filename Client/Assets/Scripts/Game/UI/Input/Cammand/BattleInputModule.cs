namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleInputModule : InputModule
    {
        public override ModuleType ModuleType => ModuleType.Battle;

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Cancel:
                    //Pop();
                    break;
                case InputType.Esc:
                    PopAll();
                    Game.System.BattleSystem.ExitBattle();
                    break;
                case InputType.Map:
                    break;
            }
        }
    }
}
