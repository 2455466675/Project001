using Game.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public struct ActionContext 
    {
        public InputType InputType { get; set; }
        public bool BoolValue { get; set; }
        public Vector2 Vector2Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class InputCammand
    {
        public bool IsStatic {get;set;}
        public int Count => subCammands.Count;

        private readonly Stack<InputCammand> subCammands;

        public InputCammand() 
        {
            subCammands = new Stack<InputCammand>();
        }

        public bool Pop() 
        {
            if (subCammands.Count == 0) 
            {
                return !IsStatic;
            }

            if (TryPeek(out InputCammand cammand)) 
            {
                bool isOver = cammand.Pop();
                if (!isOver) 
                {
                    return false;
                }

                cammand.OnPop();
                subCammands.Pop();

                if (subCammands.Count == 0) 
                {
                    return !IsStatic;
                }
                else
                {
                    if (TryPeek(out cammand)) 
                    {
                        cammand.Rise();
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 清空，直到遇到一个静态命令
        /// </summary>
        public void PopAll()
        {
            while (Count > 0)
            {
                InputCammand top = Top();
                if (top == null)
                {
                    break;
                }
                if (top.IsStatic)
                {
                    break;
                }

                Pop();
            }
        }

        public void Push(InputCammand cammand) 
        {
            if (cammand == null) 
            {
                return;
            }

            if (TryPeek(out InputCammand peek)) 
            {
                peek.Sink();
            }

            cammand.OnPush();
            subCammands.Push(cammand);
        }

        /// <summary>
        /// 最上层的一个命令
        /// </summary>
        /// <returns></returns>
        public InputCammand Top() 
        {
            InputCammand temp = this;
            InputCammand cammand = this;
            while (cammand.TryPeek(out cammand))
            {
                temp = cammand;
            }
            return temp;
        }

        public bool TryPeek(out InputCammand cammand) 
        {
            return subCammands.TryPeek(out cammand);
        }

        public void InputAction(ActionContext context)
        {
            if (TryPeek(out InputCammand cammand))
            {
                cammand.InputAction(context);
            }
            OnInputAction(context);
        }

        /// <summary>
        /// 命令升到栈顶
        /// </summary>
        private void Rise() 
        {
            OnRise();
            if (TryPeek(out InputCammand cammand))
            {
                cammand.Rise();
            }
        }

        /// <summary>
        /// 命令下沉
        /// </summary>
        private void Sink() 
        {
            OnSink();
            if (TryPeek(out InputCammand cammand))
            {
                cammand.Sink();
            }
        }

        /// <summary>
        /// 当命令弹出时
        /// </summary>
        protected virtual void OnPop() 
        {
        }
        /// <summary>
        /// 当命令入栈时
        /// </summary>
        protected virtual void OnPush() 
        {
        }
        /// <summary>
        /// 当命令升到栈顶时
        /// </summary>
        protected virtual void OnRise() 
        {
        }
        /// <summary>
        /// 当命令下沉时
        /// </summary>
        protected virtual void OnSink() 
        {
        }
        protected virtual void OnInputAction(ActionContext context) 
        {
        }
    }
}
