using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public static class MLog
	{
        public static void Log(params object[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                return;
            }      
            Debug.Log(string.Join(" ", logs));
        }

        public static void Error(object error)
        {
            Debug.LogError(error);  
        }

        public static void Warn(object warn)
        {
            Debug.LogWarning(warn);
        }
	}
}

