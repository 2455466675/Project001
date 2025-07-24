namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleInputModule : InputModule
    {
        public static BattleInputModule Instance { get; private set; }

        public BattleInputModule() : base()
        {
            Instance = this;
        }

        public override ModuleType ModuleType => ModuleType.Battle;

        protected override bool CheckIsLocked()
        {
            return true;
        }

        protected override void OnInputAction(ActionContext context)
        {
            InputType inputType = context.InputType;
            switch (inputType)
            {
                case InputType.Cancel:
                    MLog.Log("BattleInputModule Pop");
                    Pop();
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
