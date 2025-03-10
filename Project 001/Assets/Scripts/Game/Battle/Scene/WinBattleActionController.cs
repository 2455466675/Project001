using MVC;
using Navigation;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class WinBattleActionController : MonoBehaviour
	{
        public void OnClick(GuidableItem item)
        {
            GameCore.UI.Enter(UI.ListName.BattleActionList2);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DataCollection list = DataContainer.Root.FindDataCollection("Game.Fight.ActionList2");
                if (list != null)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                DataCollection list = DataContainer.Root.FindDataCollection("Game.Fight.ActionList2");
                DataContainer item = list.Append();
                int id = list.Count;
                item.SetBaseValue("id", id);
                item.SetBaseValue("name", $"Action2_{id}");
            }
        }
    }
}

