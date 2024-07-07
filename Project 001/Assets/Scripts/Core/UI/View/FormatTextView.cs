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
            int id = isValidId ? textId : DBs[0].IntValue;

            string text = GameCore.GameCfg.GetTextById(id);
            if (string.IsNullOrEmpty(text))
            {
                MLog.Log("isValidId", isValidId ? id.ToString() : DBs[0].StringValue);
                SetTextByStr(isValidId ? id.ToString() : DBs[0].StringValue);
                return;
            }

            if (!isValidId && DBs.Length == 1) 
            {
                SetTextByStr(text);
            }
            else
            {
                string[] args = new string[DBs.Length - 1];
                for (int i = 1; i < DBs.Length; i++)
                {
                    args[i] = GameCore.GameCfg.GetTextById(DBs[i].IntValue);
                }
                SetTextByStr(string.Format(text, args));
            }
        }
    }
}

