namespace Game
{
    public class GM_Cmds
    {
        public void GM_Test(string arg) 
        {
            
        }

        public void GM_Battle(string arg) 
        {
            Game.UI.Close();
            Game.System.BattleSystem.EnterBattle();
        }
    }
}