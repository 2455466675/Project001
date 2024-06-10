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
            Debug.Log($"OnSelect:{notification.guidable.CurrentGameObject.name}");
        }

        public void OnStartNewGame(UINotification notification)
        {
            Debug.Log($"OnStartNewGame:{notification.guidable.CurrentGameObject.name}");
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