using Game.UI;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLoginState : IGameState
    {
        public GameState State => GameState.LOGIN;

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("ÓÎÏ·×´Ì¬£ºµÇÂ¼");
            GameWorld.Instance.GetComponent<SceneComponent>().LoadScene("LoginScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            GameWorld.Instance.GetComponent<UIComponent>().Navigate(UIDefine.Group_ID.Login_Group);
            GameWorld.Instance.GetComponent<UIComponent>().GetNavigationGroup(UIDefine.Group_ID.Login_Group).SetUndoable(false);
        }

        public void OnExit(GameState nextGameState)
        {

        }
    }
}