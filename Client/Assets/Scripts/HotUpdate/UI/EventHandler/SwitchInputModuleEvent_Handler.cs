namespace GameFramework.UI
{
    [GameEvent]
    public class SwitchInputModuleEvent_Handler : GameEventHandlerBase<SwitchInputModuleEventArgs>
    {
        public override void Invoke(SwitchInputModuleEventArgs arg)
        {
            Game.GetModule<InputController>().Switch(arg.moduleType);
        }
    }
}