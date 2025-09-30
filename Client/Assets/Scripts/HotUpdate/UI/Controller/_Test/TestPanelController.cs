using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI
{
    [UIPanelController(PanelDefine.TestPanel2, NavigationDefine.TestList1, NavigationDefine.TestList2)]
    public class TestPanelController : PanelController
    {
        protected override void OnShow()
        {
            MDebug.Log("PanelDefine.TestPanel2 OnShow");
        }
    }
}