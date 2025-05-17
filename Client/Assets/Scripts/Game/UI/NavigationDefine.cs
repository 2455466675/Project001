namespace Game.UI
{
    public enum NavigationGroupDefine 
    {
        Undefined = 0,

        Test_Group_1 = 1,
        Test_Group_2 = 2,

        Login_Group    = 1001,
        Loading_Group  = 1002,
        GM_Group       = 1003,
        Overview_Group = 1004,
        Backpack_Group = 1005,

        Battle_Units_Group   = 1006,
        Battle_Loading_Group = 1007,
    }

    public enum NavigationListDefine
    {
        Undefined = 0,

        Test_List_1 = 1,
        Test_List_2 = 2,
        Test_List_3 = 3,

        Login_List   = 10011,
        GM_Menu_List = 10021,
        GM_Item_List = 10022,
        Overview_Menu_List = 10031,
        Backpack_Menu_List = 10041,
        Backpack_Item_List = 10042,

        Battle_Enemy_Unit_List = 10051,
        Battle_Player_Unit_List = 10052,
    }
}