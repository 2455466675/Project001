using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameInputManager : MonoBehaviour, ICore
    {
        private float t = 0;
        public IEnumerator Init()
        {
            yield return null;
        }

        public void Update()
        {
           
        }

        public void FixedUpdate()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float s = Input.GetAxisRaw("Submit");
            float c = Input.GetAxisRaw("Cancel");

            t = Mathf.Max(0f, t - Time.deltaTime);
          
            if (x != 0)
            {
                if (t <= 0f)
                {
                    //Debug.Log($"x:{x}");
                    GameCore.UI.TestH(x);
                    t = 0.2f;
                }
            }
            if (y != 0)
            {
                if (t <= 0f) 
                {
                    //Debug.Log($"y:{y}");
                    GameCore.UI.TestV(y);
                    t = 0.2f;
                }
            }
            if (s != 0)
            {
                if (t <= 0f) 
                {
                    Debug.Log($"s:{s}");
                    GameCore.UI.Submit();
                    t = 0.2f;
                }
            }
            if (c != 0)
            {
                if (t <= 0f)
                {
                    Debug.Log($"c:{c}");
                    CommandInvoker.UndoCommand();
                    t = 0.2f;
                }
            }

        }
    }
}

