
using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewController : MonoBehaviour
	{
        public void OnPackage(GuidableItem item)
        {
            MLog.Log("OnPackage");
            GameCore.UI.Enter(ListName.PackageMenu);
        }
    } 
}

