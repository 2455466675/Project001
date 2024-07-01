using System;
namespace Game.Cfg 
{
   [Serializable]
   public class LanguageCfg : CfgDataBase
   {
       public readonly string text;
       public readonly int color;
   }
}