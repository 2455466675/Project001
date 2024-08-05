namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class FormatTextView : TextView
	{
        public override void UpdateView()
        {
            if (!IsValid) return;

            bool isValidId = textId > 0;       
            int id = isValidId ? textId : MainField.IntValue;

            string text = GameCore.GameCfg.GetTextById(id);
            if (string.IsNullOrEmpty(text))
            {
                SetTextByStr(isValidId ? id.ToString() : MainField.StringValue);
                return;
            }

            if (!isValidId && FieldCount == 1) 
            {
                SetTextByStr(text);
            }
            else
            {
                string[] args = new string[FieldCount - 1];
                for (int i = 1; i < FieldCount; i++)
                {
                    args[i] = GameCore.GameCfg.GetTextById(this[i].IntValue);
                }
                SetTextByStr(string.Format(text, args));
            }
        }
    }
}

