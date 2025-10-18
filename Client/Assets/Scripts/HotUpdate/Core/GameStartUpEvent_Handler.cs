using UnityEngine;

namespace GameFramework.Core 
{
    [GameEvent]
    public class GameStartUpEvent_Handler : GameEventHandlerBase<GameStartUpEventArgs>
    {
        public override void Invoke(GameStartUpEventArgs arg)
        {
            MDebug.Log("”Œœ∑∆Ù∂Ø!");

            /*
             * Login
             * Playing
             * 
             */
            Game.GetModule<SceneManager>().LoadScene(1001);
        }
    }
}