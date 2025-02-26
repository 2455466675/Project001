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
            UIEntity = MyEntity.CreateChild();

            UIRootComponent uirc = UIEntity.AddComponent<UIRootComponent>();

            yield return uirc.Init(intCfg);
        }

        public void ShowPanel<T>(int id) where T : PanelControllerComponent, new()
        {
            UIEntity.GetComponent<UIRootComponent>().ShowPanel<T>(id);
        }
    }
}
