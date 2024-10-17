using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class GmController : MonoBehaviour
	{
        public void OnSelectedMenu(GuidableItem item)
        {
            GameCore.System.Gm.SelectMenu(item.Datum);
        }

        public void OnSubmitMenu(GuidableItem item)
        {
            GameCore.UI.Enter(ListName.GmItemList);
        }

        public void OnSubmitItem(GuidableItem item)
        {
            int id = item.Datum.GetDataBase("id").IntValue;
            if (id == 1001)
            {
                GameCore.System.ItemSystem.Package.Test();
            }
            else
            {
                GameCore.System.ItemSystem.Package.Test1();
            }
        }
    }
}

