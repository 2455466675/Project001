using System;
namespace Game.Cfg 
{
   [Serializable]
   public class RoleCfg : CfgDataBase
   {
       public readonly string name;
       public readonly string prefabPath;
       public readonly int roleType;
       public readonly string headIcon;
   }
}