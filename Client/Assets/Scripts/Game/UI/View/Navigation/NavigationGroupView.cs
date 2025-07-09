using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupView : View, IViewContainer
    {
        public NavigationListView[] Children => children;

        [SerializeField]
        private NavigationGroupDefine define;

        [ReadOnly]
        [SerializeField]
        protected NavigationListView[] children;

        [SerializeField]
        private ViewEntity[] views;

        public T GetView<T>() where T : View
        {
            if (views == null)
            {
                return default;
            }

            Type t = typeof(T);
            for (int i = 0; i < views.Length; i++)
            {
                var ve = views[i];
                if (ve == null || ve.view == null)
                {
                    continue;
                }

                if (ve.view.GetType() == t)
                {
                    return ve.view as T;
                }
            }
            return default;
        }

        public T GetView<T>(string key) where T : View
        {
            if (views == null)
            {
                return default;
            }

            Type t = typeof(T);
            for (int i = 0; i < views.Length; i++)
            {
                var ve = views[i];
                if (ve == null || ve.view == null)
                {
                    continue;
                }

                if (ve.view.GetType() == t && ve.key == key)
                {
                    return ve.view as T;
                }
            }
            return default;
        }

        [Button("Init")]
        public void Init()
        {
            if (Application.isPlaying) 
            {
                return;
            }

            GenerateMap();
            InitViews();
        }

        private void GenerateMap() 
        {
#if UNITY_EDITOR
            if (define == NavigationGroupDefine.Undefined)
            {
                Debug.LogError("NavigationGroupView is Undefined");
                return;
            }

            children = GetComponentsInChildren<NavigationListView>();

            if (children == null || children.Length == 0)
            {
                return;
            }


            NavigationMap map = Resources.Load<NavigationMap>("Game/NavigationMap");
            if (map == null)
            {
                return;
            }

            string s = "";
            foreach (var view in children)
            {
                if (!view.IsValid)
                {
                    Debug.LogError($"NavigationListView is not valid : {view.Define}");
                    continue;
                }
                if (view.Define == NavigationListDefine.Undefined)
                {
                    Debug.LogError($"NavigationListView is Undefined");
                    continue;
                }
                map.AddMap(view.Define, define);
                s += $"({view.Define} => {define});";
            }

            EditorUtility.SetDirty(map);
            AssetDatabase.SaveAssets();
            Debug.Log($"GenerateMap! : {s}");
#endif
        }

        private void InitViews() 
        {
            List<View> result = new List<View>();
            FindViews(transform, result);

            List<ViewEntity> temp = new List<ViewEntity>(result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                View view = result[i];

                ViewEntity entity = new ViewEntity();
                entity.key = string.Format("{0}_{1}", view.gameObject.name, view.GetType().Name);
                entity.view = view;
                temp.Add(entity);
            }

            views = temp.ToArray();
        }

        private void FindViews(Transform tf, List<View> result)
        {
            if (tf == null)
            {
                return;
            }

            View[] v = tf.GetComponents<View>();
            if (v != null && v.Length > 0)
            {
                foreach (var item in v)
                {
                    if (item != this) 
                    {                        
                        result.Add(item);
                    }
                }
            }

            int childCount = tf.childCount;
            if (childCount <= 0)
            {
                return;
            }

            for (int i = 0; i < childCount; i++)
            {
                Transform child = tf.GetChild(i);
                if (child.GetComponent<IViewContainer>() != null)
                {
                    child.GetComponent<IViewContainer>().Init();
                    continue;
                }

                FindViews(child, result);
            }
        }
    }
}
