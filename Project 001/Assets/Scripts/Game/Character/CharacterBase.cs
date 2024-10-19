using MVC;

namespace Game.System
{
    /// <summary>
    /// ÓÎÏ·½ÇÉ«Àà
    /// </summary>
	public class CharacterBase : DataProxy
    {
        public Actor Actor;

        public CharacterBase(DataContainer container) : base(container)
        {
                    
        }

        public void RefreshActor()
        {

        }
    }
}

