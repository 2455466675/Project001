using EC;
using Game.UI;
using System.Collections;
using System.Security.Principal;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponent : EC.Component, IInitializable
    {
        private Entity UIEntity;

        public IEnumerator Init(GameInitCfg intCfg) 
        {            
            UIEntity = Entity.CreateChild();

            UIRootComponent uirc = UIEntity.AddComponent<UIRootComponent>();

            yield return uirc.Init(intCfg);
        }

        public void ShowPanel(int id) 
        {
            UIEntity.GetComponent<UIRootComponent>().ShowPanel(id);
        }
    }
}
