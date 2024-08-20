namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public interface INavigatable
	{
        bool IsFocus { get; }
        /// <summary>
        /// ¾Û½¹
        /// </summary>
        void InFocus();
        /// <summary>
        /// Ê§½¹
        /// </summary>
        void OutFocus();
        /// <summary>
        /// ÍË³ö
        /// </summary>
        void Exit();
        void MoveUp();
        void MoveDown();
        void MoveLeft();
        void MoveRight();
    }
}

