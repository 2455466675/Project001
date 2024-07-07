using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticTextView : TextView
	{
        public void Start()
        {
            SetTextById(textId);           
        }
    }
}

