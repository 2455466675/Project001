using Navigation;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupView : MonoBehaviour
    {
        public NavigationGroupDefine define;
        public NavigationGroup Group => group;
        public NavigationListView[] Children => children;

        [SerializeField]
        private NavigationGroup group;

        [SerializeField]
        protected NavigationListView[] children;

        private void OnValidate()
        {
            if (group == null)
            {
                group = GetComponent<NavigationGroup>();
            }
        }

        [Button("GenerateMap")]
        private void GenerateMap()
        {
            if (define == NavigationGroupDefine.Undefined) 
            {
                Debug.LogError("NavigationGroupView is Undefined");
                return;
            }

            if (group == null)
            {
                group = GetComponentInChildren<NavigationGroup>();
            }
            if (group == null) 
            {
                Debug.LogError("NavigationGroup not Find");
                return;
            }

            children = GetComponentsInChildren<NavigationListView>();

            if (children == null || children.Length == 0) 
            {
                Debug.LogError("NavigationListView[] not Find");
                return;
            }

            NavigationMap map = Resources.Load<NavigationMap>("Game/NavigationMap");
            if (map == null) 
            {
                return;
            }

            group.Init();
            string s = "";
            foreach (var view in children) 
            {
                view.Init();
                if (!view.IsValid) 
                {
                    Debug.LogError($"NavigationListView is not valid : {view.define}");
                    continue;
                }
                if (view.define == NavigationListDefine.Undefined) 
                {
                    Debug.LogError($"NavigationListView is Undefined");
                    continue;
                }
                map.AddMap(view.define, define);
                s += $"({view.define} => {define});";
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(map);
            AssetDatabase.SaveAssets();
#endif

            Debug.Log($"GenerateMap! : {s}");
        }
    }
}
