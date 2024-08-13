namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface IUICommand
	{
        /// <summary>
        /// ÊÇ·ñ¿É³·Ïú
        /// </summary>
        bool IsUndoable {get;}

        void OnPush();
        void OnPop();
        void OnRise();
        void OnSink();

    }
}

