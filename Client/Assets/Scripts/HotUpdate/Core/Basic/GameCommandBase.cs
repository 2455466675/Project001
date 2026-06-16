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

        #region �����ӿ�

        /// <summary>
        /// �����������������
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
                bool isOver = command.Pop(); //�����ڲ������ϲ����һ������
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
                            return Pop();   //�����һ����������ʧ�ܣ�����Ҳ����
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
        /// ��գ�ֱ������һ����̬����
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
        /// ѹ��һ��������
        /// </summary>
        /// <param name="command"></param>
        /// <returns>�Ƿ�ɹ�</returns>
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
        /// �������һ������
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
        /// TryPeek����
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

        #region

        /// <summary>
        /// ��������ջ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
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
        /// �����³�
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

        #region ����ʵ��
        /// <summary>
        /// �������ʱ
        /// </summary>
        protected virtual void OnPop()
        {
        }
        /// <summary>
        /// ��������ջʱ
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        protected virtual bool OnPush()
        {
            return true;
        }
        /// <summary>
        /// ����������ջ��ʱ
        /// </summary>
        protected virtual bool OnRise()
        {
            return true;
        }
        /// <summary>
        /// �������³�ʱ
        /// </summary>
        protected virtual void OnSink()
        {
        }
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckLocked()
        {
            return false;
        }
        #endregion
    }
}


