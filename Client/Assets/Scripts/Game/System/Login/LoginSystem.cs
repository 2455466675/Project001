using Game.State;
using Navigation;

namespace Game.System
{
    public abstract class LoginOption : NavigationItemData
    {
        public abstract void Execute();
    }

    public class LoginSystem
    {
        #region LoginOption

        private class NewGameOption : LoginOption
        {
            public override void Execute()
            {
                MLog.Log("NewGameOption.Execute");
                Game.System.LoginSystem.LockLoginGroup = false;
                Game.State.Switch(GameStateDefine.Playing);
            }
        }

        private class LoadGameOption : LoginOption
        {
            public override void Execute()
            {
                MLog.Log("LoadGameOption.Execute");
            }
        }

        private class GameSettingOption : LoginOption
        {
            public override void Execute()
            {
                MLog.Log("GameSettingOption.Execute");
            }
        }

        private class QuitGameOption : LoginOption
        {
            public override void Execute()
            {
                MLog.Log("QuitGameOption.Execute");                    
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
#endif
            }
        }

        #endregion

        public bool LockLoginGroup { get; set; }

        private LoginOption[] options;

        public void Init()
        {
            options = new LoginOption[4];

            options[0] = new NewGameOption();
            options[1] = new LoadGameOption();
            options[2] = new GameSettingOption();
            options[3] = new QuitGameOption();
        }

        public LoginOption[] GetOptions() 
        {
            return options;
        }
    }
}