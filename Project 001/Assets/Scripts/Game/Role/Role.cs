
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

        public IntDB ID { get; private set; }

        public StringDB Name { get; private set; }

        public bool isRun;

        public Role(int id)
        {
            RoleCfg cfg = GameCore.GameCfg.Find<RoleCfg>(id);
            if (cfg == null)
            {
                return;
            }

            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.PrefabPath);
            GameObject actorObj = GoHelper.Instantiate(obj, GameCore.Scene.GetContainer(cfg.Container).container);
            Actor = actorObj.GetComponent<Actor>();

            ID = new IntDB(cfg.Id);
            Name = new StringDB(cfg.Name);
            Cfg = cfg;
        }

        public void Move(float x, float y)
        {
            string actionName;
            if (x != 0)
            {
                if (x > 0)
                {
                    if (isRun)
                    {
                        actionName = "RunRight";
                    }
                    else
                    {
                        actionName = "WalkRight";
                    }
                }
                else
                {
                    if (isRun)
                    {
                        actionName = "RunLeft";
                        Actor.PlayAction("RunLeft");
                    }
                    else
                    {
                        actionName = "WalkLeft";
                    }
                }
            }
            else if (y != 0)
            {
                if (y > 0)
                {
                    if (isRun)
                    {
                        actionName = "RunUp";
                    }
                    else
                    {
                        actionName = "WalkUp";
                    }
                }
                else
                {
                    if (isRun)
                    {   
                        actionName = "RunDown";
                    }
                    else
                    {
                        actionName = "WalkDown";
                    }
                }
            }
            else
            {   
                actionName = "Idle";
            }

            Actor.PlayAction(actionName);
            GameCore.System.RoleSystem.actionNames.Enqueue(actionName);
        }
    }
}

