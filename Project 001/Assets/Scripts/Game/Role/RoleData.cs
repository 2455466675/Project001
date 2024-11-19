using MVC;

namespace Game.System
{
    public interface IRoleData
    {
        RoleData GetRoleData();
    }

    /// <summary>
    /// 
    /// </summary>
	public class RoleData : DataProxy
    {
        public RoleData(DataContainer container) : base(container)
        {
        }
    }
}

