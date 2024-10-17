
using Game.Cfg;
using Game.Core;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class Role
	{
        public Actor Actor { get; private set; }

        public RoleData RoleData { get; private set; }

        public RoleCfg Cfg { get; private set; }

        public bool IsLeader;

        public bool IsMoving => Actor != null && Actor.IsMoving;
        public bool IsRunning => Actor != null && Actor.IsRunning;

        public Role PrevRole;

        public Role NextRole;


        public Role(int id)
        {
            RoleCfg cfg = GameCore.Cfg.Find<RoleCfg>(id);
            if (cfg == null)
            {
                return;
            }

            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.PrefabPath);
            GameObject actorObj = GoHelper.Instantiate(obj, GameCore.Scene.GetContainer(cfg.Container).container);
            Actor = actorObj.GetComponent<Actor>();

            Actor.SetRole(this);
            Cfg = cfg;
        }
       
        public void SetIsLeader(bool isLeader)
        {
            IsLeader = isLeader;
            Actor.SetColloderEnabled(isLeader);
        }

        /// <summary>
        /// ½ÇÉ«ÒÆ¶¯
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(float x, float y)
        {
            Actor.Move(x, y);     
        }     

        /// <summary>
        /// ½ÇÉ«±¼ÅÜ
        /// </summary>
        /// <param name="isRunning">ÊÇ·ñ±¼ÅÜ</param>
        public void Run(bool isRunning)
        {
            Actor.Run(isRunning);
        }
    }
}