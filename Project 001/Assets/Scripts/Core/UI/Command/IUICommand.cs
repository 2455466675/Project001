namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface IUICommand
	{
        /// <summary>
        /// 是否可撤销
        /// </summary>
        bool IsUndoable {get;}
        /// <summary>
        /// 入栈时
        /// </summary>
        void OnPush();
        /// <summary>
        /// 出栈时
        /// </summary>
        void OnPop();
        /// <summary>
        /// 回到栈顶时
        /// </summary>
        void OnRise();
        /// <summary>
        /// 不在栈顶时
        /// </summary>
        void OnSink();

    }
}

