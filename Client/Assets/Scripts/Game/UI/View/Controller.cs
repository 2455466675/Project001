using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    public class Controller : MonoBehaviour, IViewContainer
    {
        [SerializeField]
        private ViewEntity[] views;

        private void Awake()
        {
            Register();
        }

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

        public T GetView<T>(string viewName) where T : View 
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

                if (ve.view.GetType() == t && ve.name == viewName)
                {
                    return ve.view as T;
                }
            }

            return default;
        }

        protected virtual void Register() { }

        [Button("Init")]
        public void Init() 
        {
            List<View> result = new List<View>();
            FindViews(transform, result);

            List<ViewEntity> temp = new List<ViewEntity>(result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                View view = result[i];

                ViewEntity entity = new ViewEntity();
                entity.name = view.gameObject.name;
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
                result.AddRange(v);            
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
