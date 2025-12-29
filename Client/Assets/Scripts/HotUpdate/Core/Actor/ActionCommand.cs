using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public class ActionData
    {
        public IActor actor;
        public int direction;
        public List<Vector2Int> points;
        public object userData;
    }

    [Serializable]
    public abstract class ActionCommand
    {
        [SerializeField]
        public int priority;

        [SerializeField]
        public float timepoint;

        [SerializeField]
        public float duration;

        protected ActionData actionData;

        public void Execute()
        {
            OnExecute();
        }
        public void Complete()
        {
            OnComplete();
        }

        public void Init(ActionData data)
        {
            this.actionData = data;
        }

        protected virtual void OnExecute() { }
        protected virtual void OnComplete() { }
    }

    public class EmptyCommand : ActionCommand
    {

    }
}
