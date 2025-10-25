using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Gameplay
{
    [GameEvent]
    public class SaveSlotsEvent_Handler : GameEventHandlerBase<SaveSlotsEventArgs>
    {
        public override void Invoke(SaveSlotsEventArgs arg)
        {
            DataModel slot = arg.slot;
            slot.SetValue("scene_id", 1001);
        }
    }
}
