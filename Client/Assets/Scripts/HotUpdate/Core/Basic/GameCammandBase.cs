using System.Collections.Generic;

namespace GameFramework.Core
{
    public abstract class GameCammandBase
    {
        public bool IsLocked => CheckLocked();
        public int Count => subCammands.Count;

        private readonly Stack<GameCammandBase> subCammands;

        protected bool IsPopAll { get; private set; }

        public GameCammandBase()
        {
            subCammands = new Stack<GameCammandBase>();
        }

        #region 操作接口

        /// <summary>
        /// 弹出最上面的子命令
        /// </summary>
        /// <returns>cammand is clean?</returns>
        public bool Pop()
        {
            if (subCammands.Count == 0)
            {
                return !IsLocked;
            }

            if (TryPeek(out GameCammandBase cammand))
            {
                bool isOver = cammand.Pop(); //总是在操作最上层的那一个命令
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
            IsPopAll = true;
            while (Count > 0)
            {
                GameCammandBase top = Top();
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
            IsPopAll = false;
        }

        /// <summary>
        /// 压入一个子命令
        /// </summary>
        /// <param name="cammand"></param>
        /// <returns>是否成功</returns>
        public bool Push(GameCammandBase cammand)
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

            if (TryPeek(out GameCammandBase peek))
            {
                peek.Sink();
            }
            subCammands.Push(cammand);
            return true;
        }

        /// <summary>
        /// 最上面的一个命令
        /// </summary>
        /// <returns></returns>
        public GameCammandBase Top()
        {
            GameCammandBase temp = this;
            GameCammandBase cammand = this;
            while (cammand.TryPeek(out cammand))
            {
                temp = cammand;
            }
            return temp;
        }

        /// <summary>
        /// TryPeek操作
        /// </summary>
        /// <param name="cammand"></param>
        /// <returns></returns>
        //public bool TryPeek(out GameCammandBase cammand)
        //{
        //    return subCammands.TryPeek(out cammand);
        //}

        public bool TryPeek<T>(out T cammand) where T : GameCammandBase
        {
            if (Count == 0)
            {
                cammand = default;
                return false;
            }

            if (subCammands.TryPeek(out GameCammandBase gc))
            {
                if (gc is T tc)
                {
                    cammand = tc;
                    return true;
                }
            }

            cammand = default;
            return false;
        }

        #endregion

        #region

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
                if (TryPeek(out GameCammandBase cammand))
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
            if (TryPeek(out GameCammandBase cammand))
            {
                cammand.Sink();
            }
        }

        #endregion

        #region 子类实现
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
        /// <summary>
        /// 是否锁定此命令
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckLocked()
        {
            return false;
        }
        #endregion
    }
}


