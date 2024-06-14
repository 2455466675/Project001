using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WinLoginController : UIController
    {

        public void OnSelect(UINotification notification)
        {

        }

        public void OnStartNewGame(UINotification notification)
        {
            GameCore.UI.OpenWinCommond(100003);
        }

        public void OnExit(UINotification notification)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Debug.Log("OnExit");
            Application.Quit();
        }
    }
}