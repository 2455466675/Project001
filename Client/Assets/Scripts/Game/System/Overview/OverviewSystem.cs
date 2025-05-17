using Navigation;

namespace Game.GSystem
{
    public abstract class OverviewMenuBase : NavigationItemData
    {
        public abstract void Execute();
    }

    public class OverviewSystem
    {
        private class OverviewMenu_Backpack : OverviewMenuBase
        {
            public override void Execute()
            {
                Game.UI.Navigate(UI.NavigationListDefine.Backpack_Menu_List);
            }
        }

        private class OverviewMenu_Party : OverviewMenuBase
        {
            public override void Execute()
            {

            }
        }

        private class OverviewMenu_Skill : OverviewMenuBase
        {
            public override void Execute()
            {

            }
        }

        private class OverviewMenu_Job : OverviewMenuBase
        {
            public override void Execute()
            {

            }
        }

        private class OverviewMenu_Question : OverviewMenuBase
        {
            public override void Execute()
            {

            }
        }

        private class OverviewMenu_Other : OverviewMenuBase
        {
            public override void Execute()
            {

            }
        }

        private OverviewMenuBase[] menus;

        public void Init() 
        {
            menus = new OverviewMenuBase[6];

            menus[0] = new OverviewMenu_Backpack();
            menus[1] = new OverviewMenu_Party();
            menus[2] = new OverviewMenu_Skill();
            menus[3] = new OverviewMenu_Job();
            menus[4] = new OverviewMenu_Question();
            menus[5] = new OverviewMenu_Other();
        }

        public OverviewMenuBase[] GetMenus() 
        {
            return menus;
        }
    }
}