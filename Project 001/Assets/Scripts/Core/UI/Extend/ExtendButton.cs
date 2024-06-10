using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendButton : Button
    {
        private event Action<BaseEventData> OnSubmitHandler;
        private event Action<BaseEventData> OnSelectHandler;
        private event Action<BaseEventData> OnDeselectHandler;

        protected override void Awake()
        {
            base.Awake();
        }

        public void AddOnSelectListener(Action<BaseEventData> action)
        {
            OnSelectHandler += action;
        }

        public void AddOnDeselectListener(Action<BaseEventData> action)
        {
            OnDeselectHandler += action;
        }

        public void AddOnSubmitListener(Action<BaseEventData> action)
        {
            OnSubmitHandler += action;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);       
            OnSelectHandler?.Invoke(eventData);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            OnDeselectHandler?.Invoke(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            OnSubmitHandler?.Invoke(eventData);
            onClick?.Invoke();
        }

    }
}