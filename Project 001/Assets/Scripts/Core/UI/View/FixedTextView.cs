namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class FixedTextView : TextView
	{
        protected override void Start()
        {
            base.Start();
            SetTextById(textId);
        }
    }
}

