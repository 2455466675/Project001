namespace GameFramework.Core
{
    public abstract class InputModuleBase : IInputable
    {
        public abstract InputModuleType ModuleType { get; }
        public abstract void OnInput(InputContext context);        
    }
}