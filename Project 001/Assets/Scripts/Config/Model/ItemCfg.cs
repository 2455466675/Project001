using System;
namespace Game.Cfg 
{
   [Serializable]
   public class ItemCfg : CfgDataBase
   {
       public readonly string name;
       public readonly string icon;
       public readonly string desc;
   }
}