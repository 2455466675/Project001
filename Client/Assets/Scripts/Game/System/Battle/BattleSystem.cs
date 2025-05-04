using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSystem
    {
        public void Init() 
        {
        }

        public void EnterBattle() 
        {
            Game.Scene.LoadSceneAsync(10002, null, OnEnterScene);
        }

        private void OnEnterScene() 
        {
            MLog.Log("OnEnterScene");
            Game.UI.Navigate(UI.NavigationListDefine.Battle_Enemy_Unit_List, UI.Input.ModuleType.Battle);
        }
    }
}
