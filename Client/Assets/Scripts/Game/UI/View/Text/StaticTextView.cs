using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticTextView : TextView
    {
        [SerializeField]
        private int textId;

        protected void Start()
        {
            SetTextById(textId);
        }
    }
}
