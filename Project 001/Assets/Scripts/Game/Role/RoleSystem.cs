using MVC;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 玩家角色（数据）
    /// </summary>
	public class RoleSystem : DataProxy, IGameSystem
    {
        private DataCollection roles;

        private Dictionary<int, RoleData> datas;

        public RoleSystem(DataContainer container) : base(container)
        {
            datas = new Dictionary<int, RoleData>();

            roles = CreateCollection("Roles");

            for (int i = 0; i < 5; i++)
            {
                DataContainer item = roles.Append();
                int id = 1001 + i;
                item.SetBaseValue("id", id);
                item.SetBaseValue("hp", i * 100 + i);
                item.SetBaseValue("maxHp", (i + 1) * 100 + i);
                item.SetBaseValue("sp", i * 50 + i);
                datas[id] = new RoleData(item);
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public RoleData BindRoleData(int id, RoleBase role)
        {
            RoleData data;
            if (datas.TryGetValue(id, out data))
            {
                role.SetContainerLinker("RoleData", data.Container);
            }
            else
            {
                DataContainer item = roles.Append();
                item.SetBaseValue("id", id);
                data = new RoleData(item);
                datas[id] = data;
            }

            return data;
        }

        public RoleData GetRoleData(int id)
        {
            if (datas.ContainsKey(id))
            {
                return datas[id];
            }
            else
            {
                return null;
            }
        }
    }
}

