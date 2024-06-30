using System.Text;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public static class MLog
	{
        public static void Log(params string[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                return;
            }      
            Debug.Log(string.Join(" ", logs));
        }

        public static void Error(string error)
        {
            Debug.LogError(error);  
        }

        public static void Warn(string warn)
        {
            Debug.LogWarning(warn);
        }
	}
}

