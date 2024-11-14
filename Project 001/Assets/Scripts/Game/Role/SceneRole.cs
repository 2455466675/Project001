using Game.Cfg;
using MVC;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 场景角色
    /// </summary>
	public class SceneRole : RoleBase
    {
        public int Id { get; private set; }
        public RoleCfg Cfg { get; private set; }

        public bool IsMoving => Actor != null && Actor.IsMoving;
        public bool IsRunning => Actor != null && Actor.IsRunning;

        /// <summary>
        /// 是否是队长
        /// </summary>
        public bool IsLeader { get; set; }
        /// <summary>
        /// 前一个角色（队长为null）
        /// </summary>
        public SceneRole PrevRole { get; set; }
        /// <summary>
        /// 后一个角色（队尾为null）
        /// </summary>
        public SceneRole NextRole { get; set; }

        public SceneRole(int id, DataContainer container) : base(container)
        {
            Id = id;
            Cfg = GameCore.Cfg.Find<RoleCfg>(id);
            if (Cfg == null)
            {
                MLog.Error($"SceneRole：{id}");
            }
            RefreshActor();
        }

        public override void RefreshActor()
        {
            if (Cfg == null)
            {
                return;
            }

            GameObject obj = GameCore.ResourceManager.LoadAndInstantiate(Cfg.PrefabPath, GameCore.Scene.GetContainer(Cfg.Container).container);  
            Actor = obj.GetComponent<Actor>();   
            Actor.SetRole(this);
        }

        /// <summary>
        /// 角色移动
        /// </summary>
        /// <param name="dir"></param>
        public void Move(Vector2 dir)
        {
            Actor.Move(dir);
        }

        /// <summary>
        /// 奔跑
        /// </summary>
        /// <param name="isRunning">是否奔跑</param>
        public void Run(bool isRunning)
        {
            Actor.Run(isRunning);
        }
    }
}

