using GameFramework.Core;
using GameFramework.Featrue;

namespace GameFramework.Gameplay
{
    [GameEvent]
    public class BattleCameraRotate_Handler : GameEventHandlerBase<CameraRotateArgs>
    {
        public override void Invoke(CameraRotateArgs arg)
        {
            Entity entity = Game.GetSystem<BattleSystem>().Entity;
            var ac = entity.GetComponent<ActorComponent>();
            ac.SyncRotation();
        }
    }
}
