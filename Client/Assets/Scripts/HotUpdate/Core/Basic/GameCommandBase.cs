using System.Collections.Generic;

namespace GameFramework.Core
{
    public abstract class GameCommandBase
    {
        public bool IsLocked => CheckLocked();
        public int Count => subCommands.Count;

        private readonly Stack<GameCommandBase> subCommands;

        protected bool IsPopAll { get; private set; }

        public GameCommandBase()
        {
            subCommands = new Stack<GameCommandBase>();
        }

        #region 公共接口

        /// <summary>
        /// 弹出栈顶命令（自内层向外递归，一次只真正弹出最内层的一个命令）
        /// </summary>
        /// <returns>command is clean?</returns>
        public bool Pop()
        {
            if (subCommands.Count == 0)
            {
                return !IsLocked;
            }

            if (TryPeek(out GameCommandBase command))
            {
                bool isOver = command.Pop(); // 先递归处理内层，内层弹空后才轮到本层
                if (!isOver)
                {
                    return false;
                }

                command.OnPop();
                subCommands.Pop();

                if (subCommands.Count == 0)
                {
                    return !IsLocked;
                }
                else
                {
                    if (TryPeek(out command))
                    {
                        bool success = command.Rise();
                        if (!success)
                        {
                            return Pop();   // 新栈顶上浮失败视为不可停留，连带把它也弹出
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
        /// 连续弹出，直到遇到被锁定的命令为止
        /// </summary>
        public void PopAll()
        {
            IsPopAll = true;
            while (Count > 0)
            {
                GameCommandBase top = Top();
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
        /// <param name="command"></param>
        /// <returns>是否成功</returns>
        public bool Push(GameCommandBase command)
        {
            if (command == null)
            {
                return false;
            }
            bool success = command.OnPush();
            if (!success)
            {
                return false;
            }

            if (TryPeek(out GameCommandBase peek))
            {
                peek.Sink();
            }
            subCommands.Push(command);
            return true;
        }

        /// <summary>
        /// 返回最内层的栈顶命令
        /// </summary>
        /// <returns></returns>
        public GameCommandBase Top()
        {
            GameCommandBase temp = this;
            GameCommandBase command = this;
            while (command.TryPeek(out command))
            {
                temp = command;
            }
            return temp;
        }

        /// <summary>
        /// TryPeek 封装
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //public bool TryPeek(out GameCommandBase command)
        //{
        //    return subCommands.TryPeek(out command);
        //}

        public bool TryPeek<T>(out T command) where T : GameCommandBase
        {
            if (Count == 0)
            {
                command = default;
                return false;
            }

            if (subCommands.TryPeek(out GameCommandBase gc))
            {
                if (gc is T tc)
                {
                    command = tc;
                    return true;
                }
            }

            command = default;
            return false;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 重新回到栈顶激活状态
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
                if (TryPeek(out GameCommandBase command))
                {
                    return command.Rise();
                }
                return true;
            }
        }

        /// <summary>
        /// 被新命令压入后下沉
        /// </summary>
        private void Sink()
        {
            OnSink();
            if (TryPeek(out GameCommandBase command))
            {
                command.Sink();
            }
        }

        #endregion

        #region 子类可重写
        /// <summary>
        /// 命令被弹出时
        /// </summary>
        protected virtual void OnPop()
        {
        }
        /// <summary>
        /// 命令入栈时
        /// </summary>
        /// <returns>是否成功</returns>
        protected virtual bool OnPush()
        {
            return true;
        }
        /// <summary>
        /// 命令重新回到栈顶时
        /// </summary>
        protected virtual bool OnRise()
        {
            return true;
        }
        /// <summary>
        /// 命令下沉时
        /// </summary>
        protected virtual void OnSink()
        {
        }
        /// <summary>
        /// 是否锁定（锁定时不允许被弹出）
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckLocked()
        {
            return false;
        }
        #endregion
    }
}
