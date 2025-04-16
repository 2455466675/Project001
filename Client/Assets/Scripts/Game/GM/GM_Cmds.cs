namespace Game
{
    public class GM_Cmds
    {
        public void GM_Test(string arg) 
        {
            MLog.Log($"GM_Test : {arg}");
        }

        public void GM_Battle(string arg) 
        {
            MLog.Log($"GM_Battle : {arg}");
        }
    }
}