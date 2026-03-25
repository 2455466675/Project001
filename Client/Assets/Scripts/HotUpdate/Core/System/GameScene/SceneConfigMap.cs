using UnityEngine;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [CreateAssetMenu(menuName = "MyMenu/Create SceneMap")]
    public class SceneConfigMap : ScriptableObject
    {
        [SerializeField]
        private List<SceneConfig> data;

        internal SceneConfig GetConfig(int id)
        {
            if (data == null || data.Count == 0)
            {
                return null;
            }

            return data.Find(s => s.Id == id);
        }

        internal List<SceneConfig> GetConfigList()
        {
            if (data == null)
            {
                return new List<SceneConfig>();
            }
            else
            {
                return data;                
            }
        }
    }
}