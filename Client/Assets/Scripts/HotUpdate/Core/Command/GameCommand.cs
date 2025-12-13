using System.Collections.Generic;

namespace GameFramework.Core 
{
    public abstract class GameCommand
    {
        public bool IsLocked => CheckLocked();
        public int Count => subCommands.Count;

        private readonly Stack<GameCommand> subCommands;

        protected bool IsPopAll { get; private set; }

        public GameCommand()
        {
            subCommands = new Stack<GameCommand>();
        }

        #region 操作接口

        /// <summary>
        /// 弹出最上面的子命令
        /// </summary>
        /// <returns>command is clean?</returns>
        public bool Pop()
        {
            if (subCommands.Count == 0)
            {
                return !IsLocked;
            }

            if (TryPeek(out GameCommand command))
            {
                bool isOver = command.Pop(); //总是在操作最上层的那一个命令
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
                GameCommand top = Top();
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
        public bool Push(GameCommand command)
        {
            if (command == null)
            {
                return false;
            }

            GameCommand peek = null;
            if (TryPeek(out peek))
            {
                peek.Sink();
            }

            subCommands.Push(command);
            bool success = command.OnPush();
            if (!success)
            {
                subCommands.Pop();
                peek?.Rise();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 命令树最上面的一个命令
        /// </summary>
        /// <returns></returns>
        public GameCommand Top()
        {
            GameCommand temp = this;
            GameCommand command = this;
            while (command.TryPeek(out command))
            {
                temp = command;
            }
            return temp;
        }

        /// <summary>
        /// TryPeek操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //public bool TryPeek(out GameCammand cammand)
        //{
        //    return subCammands.TryPeek(out cammand);
        //}

        public bool TryPeek<T>(out T command) where T : GameCommand
        {
            if (Count == 0) 
            {
                command = default;
                return false;
            }

            if (subCommands.TryPeek(out GameCommand gc))
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

        public void Clear()
        {
            if (Count > 0)
            {
                foreach (var sub in subCommands)
                {
                    sub.Clear();
                }
            }

            subCommands.Clear();
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
                if (TryPeek(out GameCommand command))
                {
                    return command.Rise();
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
            if (TryPeek(out GameCommand command))
            {
                command.Sink();
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
            MDebug.Log("virtual OnSink");
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

    public class CommandStack : GameCommand
    {

    }
}