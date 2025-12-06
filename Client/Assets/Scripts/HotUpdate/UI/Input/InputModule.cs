using GameFramework.Core;

namespace GameFramework.UI
{
    public abstract class InputModule : InputCammand
    {
        public abstract InputModuleType ModuleType { get; }

        //protected override void OnInputAction(InputContext context)
        //{
        //}
    }
}
