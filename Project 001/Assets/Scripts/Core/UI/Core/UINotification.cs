using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UINotification
    {
        public IGuidable guidable;

        public UINotification(IGuidable guidable)
        {
            this.guidable = guidable;
        }
    }
}