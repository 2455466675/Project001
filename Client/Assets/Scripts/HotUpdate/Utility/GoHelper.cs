using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Utility
{
    public static class GoHelper
    {
        private static List<GameObject> gDontDestroyGameObjects = new List<GameObject>();

        private static char[] gSeps = new char[2]
        {
        '/',
        '.'
        };

        public static bool hideGlobalGameObject
        {
            get;
            private set;
        }

        public static List<GameObject> dontDestroyGameObjects => gDontDestroyGameObjects;

        public static void SetHideGlobalGameObject(bool value)
        {
            hideGlobalGameObject = value;
        }

        public static void DontDestroy(GameObject go)
        {
            if (!(go == null))
            {
                gDontDestroyGameObjects.Add(go);
                UnityEngine.Object.DontDestroyOnLoad(go);
                if (hideGlobalGameObject)
                {
                    go.hideFlags |= HideFlags.HideInHierarchy;
                }
            }
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go == null)
            {
                return null;
            }

            T val = go.GetComponent<T>();
            if ((UnityEngine.Object)val == (UnityEngine.Object)null)
            {
                val = go.AddComponent<T>();
            }

            return val;
        }

        public static string CalTransformPath(Transform child, Transform parent, string connector = "/")
        {
            if (child == null)
            {
                return string.Empty;
            }

            string text = child.name;
            if (child == parent)
            {
                return text;
            }

            Transform parent2 = child.parent;
            while (parent2 != null)
            {
                text = parent2.name + connector + text;
                if (parent2 == parent)
                {
                    break;
                }

                parent2 = parent2.parent;
            }

            return text;
        }

        public static void ClearChilds(Transform parent, bool imm = false)
        {
            if (parent == null)
            {
                return;
            }

            if (Application.isEditor && imm)
            {
                while (parent.childCount > 0)
                {
                    UnityEngine.Object.DestroyImmediate(parent.GetChild(0).gameObject);
                }

                return;
            }

            for (int num = parent.childCount - 1; num >= 0; num--)
            {
                Transform child = parent.GetChild(num);
                Destroy(child.gameObject);
            }
        }

        public static Transform FindOrCreateChild(this Transform self, string target)
        {
            if (self == null)
            {
                return null;
            }

            Transform transform = self.Find(target);
            if (transform == null)
            {
                string[] array = target.Split(gSeps);
                Transform transform2 = self;
                string[] array2 = array;
                foreach (string text in array2)
                {
                    transform = transform2.Find(text);
                    if (transform == null)
                    {
                        transform = CreateGameObject(transform2, text).transform;
                    }

                    transform2 = transform;
                }
            }

            return transform;
        }

        public static Transform FindChildR(this Transform self, string target)
        {
            if (string.Equals(self.name, target))
            {
                return self;
            }

            int i = 0;
            for (int childCount = self.childCount; i < childCount; i++)
            {
                Transform transform = self.GetChild(i).FindChildR(target);
                if (transform != null)
                {
                    return transform;
                }
            }

            return null;
        }

        private static void FillChild(Transform parent, List<Transform> r, bool recursion)
        {
            int i = 0;
            for (int childCount = parent.childCount; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                r.Add(child);
                if (recursion)
                {
                    FillChild(child, r, recursion);
                }
            }
        }

        public static List<Transform> GetAllChilds(this Transform parent, bool recursion = true)
        {
            List<Transform> list = new List<Transform>();
            FillChild(parent, list, recursion);
            return list;
        }

        public static T GetMyComponentInChildren<T, TSelf>(this Component self, bool includeInactive) where T : Component where TSelf : Component
        {
            T[] componentsInChildren = self.GetComponentsInChildren<T>(includeInactive);
            T[] array = componentsInChildren;
            foreach (T val in array)
            {
                TSelf componentInParent = val.GetComponentInParent<TSelf>(includeInactive: true);
                if ((UnityEngine.Object)componentInParent != (UnityEngine.Object)null && componentInParent.gameObject == self.gameObject)
                {
                    return val;
                }
            }

            return null;
        }

        public static T[] GetMyComponentsInChildren<T, TSelf>(this Component self, bool includeInactive) where T : Component where TSelf : class
        {
            T[] componentsInChildren = self.GetComponentsInChildren<T>(includeInactive);
            List<T> list = new List<T>();
            TSelf val = self as TSelf;
            T[] array = componentsInChildren;
            foreach (T val2 in array)
            {
                TSelf componentInParent = val2.GetComponentInParent<TSelf>(includeInactive: true);
                if (componentInParent != null && componentInParent == val)
                {
                    list.Add(val2);
                }
            }

            return list.ToArray();
        }

        public static T GetComponentInParent<T>(this GameObject go, bool includeInactive)
        {
            if (go == null)
            {
                return default(T);
            }

            if (go.activeInHierarchy && !includeInactive)
            {
                return go.GetComponentInParent<T>();
            }

            T val = default(T);
            Transform transform = go.transform;
            while (transform != null && (transform.gameObject.activeSelf || includeInactive))
            {
                val = transform.GetComponent<T>();
                if (val != null)
                {
                    return val;
                }

                transform = transform.parent;
            }

            return default(T);
        }

        public static Component GetComponentInParent(this GameObject go, Type type, bool includeInactive)
        {
            if (go == null)
            {
                return null;
            }

            if (go.activeInHierarchy && !includeInactive)
            {
                return go.GetComponentInParent(type);
            }

            Component component = null;
            Transform transform = go.transform;
            while (transform != null && (transform.gameObject.activeSelf || includeInactive))
            {
                component = transform.GetComponent(type);
                if (component != null)
                {
                    return component;
                }

                transform = transform.parent;
            }

            return null;
        }

        public static T GetComponentInParent<T>(this Component c, bool includeInactive)
        {
            return c.gameObject.GetComponentInParent<T>(includeInactive);
        }

        public static Component GetComponentInParent(this Component c, Type type, bool includeInactive)
        {
            return c.gameObject.GetComponentInParent(type, includeInactive);
        }

        public static void RemoveInactiveChild(this Transform tf)
        {
            if (tf == null)
            {
                return;
            }

            for (int num = tf.childCount - 1; num >= 0; num--)
            {
                Transform child = tf.GetChild(num);
                if (child.gameObject.activeSelf)
                {
                    child.RemoveInactiveChild();
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public static void RemoveInactiveChildImm(this Transform tf)
        {
            if (tf == null)
            {
                return;
            }

            for (int num = tf.childCount - 1; num >= 0; num--)
            {
                Transform child = tf.GetChild(num);
                if (child.gameObject.activeSelf)
                {
                    child.RemoveInactiveChild();
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }
        }

        public static GameObject CreateGameObject(Transform parent, string name)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent, worldPositionStays: false);
            return gameObject;
        }

        public static GameObject CreateGameObject(Transform parent, string name, Vector3 localPos)
        {
            GameObject gameObject = CreateGameObject(parent, name);
            gameObject.transform.localPosition = localPos;
            return gameObject;
        }

        public static GameObject CreateGameObject(Transform parent, string name, Vector3 localPos, Vector3 localEulerAngles)
        {
            GameObject gameObject = CreateGameObject(parent, name, localPos);
            gameObject.transform.localEulerAngles = localEulerAngles;
            return gameObject;
        }

        public static GameObject CreateGameObject(Transform parent, string name, Vector3 localPos, Vector3 localEulerAngles, Vector3 localScale)
        {
            GameObject gameObject = CreateGameObject(parent, name, localPos, localEulerAngles);
            gameObject.transform.localScale = localScale;
            return gameObject;
        }

        public static GameObject Instantiate(UnityEngine.Object original, Transform parent)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(original, parent, instantiateInWorldSpace: false) as GameObject;
            gameObject.name = original.name;
            return gameObject;
        }

        public static GameObject Instantiate(UnityEngine.Object original, Transform parent, Vector3 localPos)
        {
            GameObject gameObject = Instantiate(original, parent);
            gameObject.transform.localPosition = localPos;
            return gameObject;
        }

        public static GameObject Instantiate(UnityEngine.Object original, Transform parent, Vector3 localPos, Quaternion localRot)
        {
            GameObject gameObject = Instantiate(original, parent, localPos);
            gameObject.transform.localRotation = localRot;
            return gameObject;
        }

        public static GameObject Instantiate(UnityEngine.Object original, Transform parent, Vector3 localPos, Quaternion localRot, Vector3 localScale)
        {
            GameObject gameObject = Instantiate(original, parent, localPos, localRot);
            gameObject.transform.localScale = localScale;
            return gameObject;
        }

        public static T Instantiate<T>(UnityEngine.Object original, Transform parent) where T : Component
        {
            UnityEngine.Object original2 = (original is Component) ? ((Component)original).gameObject : original;
            GameObject gameObject = UnityEngine.Object.Instantiate(original2, parent) as GameObject;
            gameObject.name = original.name;
            T val = gameObject.GetComponent<T>();
            if ((UnityEngine.Object)val == (UnityEngine.Object)null)
            {
                val = gameObject.AddComponent<T>();
            }

            return val;
        }

        public static T Instantiate<T>(UnityEngine.Object original, Transform parent, Vector3 localPos) where T : Component
        {
            T val = Instantiate<T>(original, parent);
            val.transform.localPosition = localPos;
            return val;
        }

        public static T Instantiate<T>(UnityEngine.Object original, Transform parent, Vector3 localPos, Quaternion localRot) where T : Component
        {
            T val = Instantiate<T>(original, parent, localPos);
            val.transform.localRotation = localRot;
            return val;
        }

        public static T Instantiate<T>(UnityEngine.Object original, Transform parent, Vector3 localPos, Quaternion localRot, Vector3 scale) where T : Component
        {
            T val = Instantiate<T>(original, parent, localPos);
            val.transform.localRotation = localRot;
            val.transform.localScale = scale;
            return val;
        }

        public static T FindComponentInParent<T>(GameObject go) where T : Component
        {
            if (go == null)
            {
                return null;
            }

            Transform transform = go.transform;
            T component = transform.GetComponent<T>();
            while ((UnityEngine.Object)component == (UnityEngine.Object)null && transform.parent != null)
            {
                transform = transform.parent;
                component = transform.GetComponent<T>();
            }

            return component;
        }

        public static T FindComponentInChild<T>(this Transform transform, string name) where T : Component
        {
            if (transform == null)
            {
                return null;
            }

            Transform transform2 = transform.Find(name);
            if (transform2 != null)
            {
                return transform2.GetComponent<T>();
            }

            return null;
        }

        public static void Foreach(this GameObject self, bool r, Action<GameObject> act)
        {
            Transform transform = self.transform;
            int i = 0;
            for (int childCount = transform.childCount; i < childCount; i++)
            {
                GameObject gameObject = transform.GetChild(i).gameObject;
                if (r)
                {
                    gameObject.Foreach(r, act);
                }

                act(gameObject);
            }
        }

        public static void SetLayer(this GameObject self, int layer, bool includeChild = true)
        {
            if (self == null)
            {
                return;
            }

            self.layer = layer;
            if (!includeChild)
            {
                return;
            }

            Transform transform = self.transform;
            foreach (Transform item in transform)
            {
                item.gameObject.SetLayer(layer);
            }
        }

        public static void Destroy(GameObject go) 
        {
            if (go == null) 
            {
                return;
            }
            UnityEngine.Object.Destroy(go);
        }
    }
}
