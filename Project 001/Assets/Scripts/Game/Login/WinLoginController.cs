using Game.Core;
using UnityEngine;
using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WinLoginController : MonoBehaviour
    {

        public void OnSelect(GuidableItem item)
        {

        }

        public void OnStartNewGame(GuidableItem item)
        {
            GameCore.UI.Exit();
            GameCore.StateController.SwitchState(GameState.PLAYING);
        }

        public void OnTest(GuidableItem item) 
        {
            //GameCore.UI.Enter(WindowId.WinOverview);
        }

        public void OnExit(GuidableItem item)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            MLog.Log("OnExit");
            Application.Quit();
        }
    }
}