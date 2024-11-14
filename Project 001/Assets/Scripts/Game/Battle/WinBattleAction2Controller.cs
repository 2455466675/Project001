using MVC;
using Navigation;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class WinBattleAction2Controller : MonoBehaviour
	{
        public void OnClick(GuidableItem item)
        {
            GameCore.UI.Enter(UI.ListName.BattleEnemyList);
        }
    }
}

