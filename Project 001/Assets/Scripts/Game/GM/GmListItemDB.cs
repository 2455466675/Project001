using Game.Core;


namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GmListItemDB : ItemDB
    {
        public int cmdId;
        public StringDB name;
        public string args;

        public GmListItemDB(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }
            string[] args = cmd.Split(':');
            name = new StringDB(args[0]);
            cmdId = int.Parse(args[1]);
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is GmListItemDB itemDB)
            {
                return cmdId.CompareTo(itemDB.cmdId);
            }
            else
            {
                return 0;
            }
        }
    }
}

