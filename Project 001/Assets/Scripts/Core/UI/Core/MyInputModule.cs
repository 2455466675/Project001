using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class MyInputModule : StandaloneInputModule
    {
        public override void Process()
        {
            //base.Process();

            //if (!eventSystem.isFocused && ShouldIgnoreEventsOnNoFocus())
            //   return;

            bool usedEvent = SendUpdateEventToSelectedObject();

            // 忽略掉触摸和点击事件
            // case 1004066 - touch / mouse events should be processed before navigation events in case
            // they change the current selected gameobject and the submit button is a touch / mouse button.

            // touch needs to take precedence because of the mouse emulation layer
            //if (!ProcessTouchEvents() && input.mousePresent)
            //   ProcessMouseEvent();

            if (eventSystem.sendNavigationEvents)
            {
                if (!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if (!usedEvent)
                    SendSubmitEventToSelectedObject();
            }
        }
    }
}