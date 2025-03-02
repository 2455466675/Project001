namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface INavigationCommand
	{
        /// <summary>
        /// 是否可撤销
        /// </summary>
        bool IsUndoable { get; }
        /// <summary>
        /// 出栈时
        /// </summary>
        void OnPop();
        /// <summary>
        /// 入栈时
        /// </summary>
        bool OnPush();
        /// <summary>
        /// 回到栈顶时
        /// </summary>
        bool OnRise();
        /// <summary>
        /// 被覆盖时
        /// </summary>
        bool OnSink();
    }
}

