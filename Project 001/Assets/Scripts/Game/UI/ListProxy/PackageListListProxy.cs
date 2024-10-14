using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageListListProxy : ListProxy
    {
        public override ListName Name => ListName.PackageList;

        public override WindowId WindowId => WindowId.WinPackage;
    }
}

