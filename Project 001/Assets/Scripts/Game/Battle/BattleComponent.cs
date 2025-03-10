using EC;
using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleComponent : EC.Component, IAwake
    {
        public void Awake()
        {
            MyEntity.AddComponent<RoundComponent>();
        }

        public void Enter() 
        {
            MyWorld.GetComponent<SceneComponent>().LoadSceneAsync("BattleScene", UnityEngine.SceneManagement.LoadSceneMode.Single, LoadingHandler, LoadEndHandler);
        }

        private void LoadEndHandler(Core.SceneInfo info)
        {
            MyWorld.GetComponent<UIComponent>().ShowPanel(UI.UIDefine.Panel_ID.Battle_Panel);
        }

        private void LoadingHandler(float obj)
        {

        }
    }
}
