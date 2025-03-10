using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WinLoginController : MonoBehaviour
    {
        [SerializeField]
        private StaticNavigationGroup loginMenu;

        private void Awake()
        {
            loginMenu.Init();
        }

        public void OnStartNewGame(NavigationItem item)
        {
            GameWorld.Instance.GetComponent<GSMComponent>().SwitchState(GameState.PLAYING);
        }

        public void OnLoadGame(NavigationItem item) 
        {
            var uic = GameWorld.Instance.GetComponent<UIComponent>();
            uic.Navigate(UIDefine.Group_ID.Test_Group_1);
        }

        public void OnQuitGame(NavigationItem item)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            MLog.Log("OnExit");
            Application.Quit();
        }
    }
}