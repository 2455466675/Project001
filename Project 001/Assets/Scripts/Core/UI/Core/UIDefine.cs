namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIDefine
    {
        public enum Panel_ID 
        {
            Undefined     = 0,
            Test_Panel    = 1,
            Test_Panel_2  = 2,

            Login_Panel    = 1001,
            Package_Panel  = 1002,
            Overview_Panel = 1003,
            Battle_Panel   = 1004,
        }

        public enum Group_ID 
        {
            Undefined    = 0,
            Test_Group_1 = 1,
            Test_Group_2 = 2,
            Test_Group_3 = 3,

            Login_Group  = 2001,
            Package_Menu_Group  = 2101,
            Package_Item_Group  = 2102,
            Overview_Menu_Group = 2201,
            Battle_Player_Group = 2301,
            Battle_Enemy_Group  = 2302,
        }
    }
}
