namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticTextView : TextView
	{
        protected override void Start()
        {
            base.Start();
            SetTextById(textId);
        }
    }
}

