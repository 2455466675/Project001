using Config;
using UnityEngine;

namespace Game.System
{
    public class ActorComponent
    {
        public int id;

        private Actor actor;

        private float dirX;
        private float dirY;
        private bool isRunning;

        public void Init(int id) 
        {
            this.id = id;
        }

        public void Refresh() 
        {
            ActorCfg cfg = Game.Config.Find<ActorCfg>(id);
            GameObject go = Game.Resource.LoadAndInstantiate(cfg.PrefabPath, Game.System.RoleSystem.Container.transform);
            actor = go.GetComponent<Actor>();
            dirX = 0f;
            dirY = -1f;
        }

        public void Move(float x, float y) 
        {
            if (x == 0f && y == 0f)
            {
                if (dirX != 0f)
                {
                    if (dirX < 0f)
                    {
                        PlayAction("IdleLeft");
                    }
                    else
                    {
                        PlayAction("IdleRight");
                    }
                }
                else
                {
                    if (dirY < 0f)
                    {
                        PlayAction("IdleDown");
                    }
                    else
                    {
                        PlayAction("IdleUp");
                    }
                }
            }
            else
            {
                if (x != 0f)
                {
                    if (x < 0f)
                    {
                        if (isRunning) 
                        {
                            PlayAction("RunLeft");
                        }
                        else
                        {
                            PlayAction("MoveLeft");                            
                        }
                    }
                    else
                    {
                        if (isRunning)
                        {
                            PlayAction("RunRight");
                        }
                        else
                        {
                            PlayAction("MoveRight");
                        }
                    }
                }
                else
                {
                    if (y < 0f)
                    {
                        if (isRunning) 
                        {
                            PlayAction("RunDown");
                        }
                        else
                        {
                            PlayAction("MoveDown");                            
                        }

                    }
                    else
                    {
                        if (isRunning) 
                        {
                            PlayAction("RunUp");
                        }
                        else
                        {
                            PlayAction("MoveUp");                            
                        }
                    }
                }
            }
            dirX = x;
            dirY = y;
        }

        public void Run(bool isRunning) 
        {
            this.isRunning = isRunning;
        }

        public void PlayAction(string actionName) 
        {
            actor.PlayAction(actionName);
        }

        protected void OnDestroy()
        {
            actor = null;
        }
    }
}