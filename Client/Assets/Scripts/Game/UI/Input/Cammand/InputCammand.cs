using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Input
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
        public bool IsLocked => CheckIsLocked();
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
                return !IsLocked;
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
                    return !IsLocked;
                }
                else
                {
                    if (TryPeek(out cammand)) 
                    {
                        bool success = cammand.Rise();  
                        if (!success) 
                        {
                            return Pop();   //如果下一个命令上升失败，将其也弹出
                        }
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
                if (top.IsLocked)
                {
                    break;
                }

                Pop();
            }
        }

        /// <summary>
        /// 压入一个子命令
        /// </summary>
        /// <param name="cammand"></param>
        /// <returns>是否成功</returns>
        public bool Push(InputCammand cammand) 
        {
            if (cammand == null) 
            {
                return false;
            }
            bool success = cammand.OnPush();
            if (!success) 
            {
                return false;
            }

            if (TryPeek(out InputCammand peek)) 
            {
                peek.Sink();
            }
            subCammands.Push(cammand);
            return true;
        }

        /// <summary>
        /// 整颗树上最上面的一个命令
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

        /// <summary>
        /// 输入操作
        /// </summary>
        /// <param name="context"></param>
        public void InputAction(ActionContext context)
        {
            if (TryPeek(out InputCammand cammand))
            {
                cammand.InputAction(context);
            }
            OnInputAction(context);
        }

        public bool TryPeek(out InputCammand cammand) 
        {
            return subCammands.TryPeek(out cammand);
        }

        /// <summary>
        /// 命令升到栈顶
        /// </summary>
        /// <returns>是否成功</returns>
        private bool Rise() 
        {
            bool success = OnRise();
            if (!success) 
            {
                return false;
            }
            else
            {
                if (TryPeek(out InputCammand cammand))
                {
                   return cammand.Rise();
                }                
                return true;
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
        /// <returns>是否成功</returns>
        protected virtual bool OnPush() 
        {
            return true;
        }
        /// <summary>
        /// 当命令升到栈顶时
        /// </summary>
        protected virtual bool OnRise() 
        {
            return true;
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
        /// <summary>
        /// 是否锁定此命令
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIsLocked() 
        {
            return false;
        }
    }
}
