namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class FormatTextView : TextView
	{
        protected override void OnUpdateView()
        {
            bool isValidId = textId > 0;
            int id = isValidId ? textId : MainField.GetIntValue();

            string text = GameCore.GameCfg.GetTextById(id);
            if (string.IsNullOrEmpty(text))
            {
                SetTextByStr(isValidId ? id.ToString() : MainField.GetStringValue());
                return;
            }

            if (!isValidId && Count == 1)
            {
                SetTextByStr(text);
            }
            else
            {
                string[] args = new string[Count - 1];
                for (int i = 1; i < Count; i++)
                {
                    args[i] = GameCore.GameCfg.GetTextById(this[i].GetIntValue());
                }
                SetTextByStr(string.Format(text, args));
            }
        }
    }
}

