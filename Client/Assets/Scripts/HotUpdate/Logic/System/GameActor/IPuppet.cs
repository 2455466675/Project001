using UnityEngine;

namespace GameFramework.Logic
{
    public struct PuppetWidgetArgs
    {
        public string assetPath;
        public string boneName;
        public string key;
        public float duration;
        public Vector3 scale;
        public Vector3 rotation;
    }

    public interface IPuppet
    {
        void SetAnimatorController(string controllerName);
        void SetAnimatorValue(string name, bool value);
        void SetAnimatorValue(string name, float value);
        void SetAnimatorValue(string name, int value);
        void SetAnimatorValue(string name);
        float PlayAnimation(string name);

        void AddWidget(PuppetWidgetArgs args);       
        void RemoveWidget(string key);
    }
}
