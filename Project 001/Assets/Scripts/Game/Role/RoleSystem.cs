
using Game.Core;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleSystem : IGameSystem
	{
        public bool isRun;
        public Actor Actor { get; private set; }

        public void Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>("Assets/Bundles/Role/PlayerActor/actor1002");
            GameObject actorObj = GoHelper.Instantiate(obj, null);
            Actor = actorObj.GetComponent<Actor>();
        }

        public void Move(float x, float y)
        {
            if (x != 0)
            {
                if (x > 0)
                {
                    if (isRun)
                    {
                        Actor.PlayAction("RunRight");
                    }
                    else
                    {
                        Actor.PlayAction("WalkRight");
                    }
                }
                else
                {
                    if (isRun)
                    {
                        Actor.PlayAction("RunLeft");
                    }
                    else
                    {
                        Actor.PlayAction("WalkLeft");
                    }
                }
            }
            else if (y != 0)
            {
                if (y > 0)
                {
                    if (isRun)
                    {
                        Actor.PlayAction("RunUp");
                    }
                    else
                    {
                        Actor.PlayAction("WalkUp");
                    }
                }
                else
                {
                    if (isRun)
                    {
                        Actor.PlayAction("RunDown");
                    }
                    else
                    {
                        Actor.PlayAction("WalkDown");
                    }
                }
            }
            else
            {
                Actor.PlayAction("Idle");
            }
        }
    }
}

