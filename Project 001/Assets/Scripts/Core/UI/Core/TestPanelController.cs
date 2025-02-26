using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TestPanelController : PanelControllerComponent
    {
        public override void Show()
        {
            PanelComponent panelComponent = GetComponent<PanelComponent>();

            var group = panelComponent.GetNavigationGroup(0);
            var lg = group.group as LoopNavigationGroup;
            lg.Init(5);
        }
    }
}
