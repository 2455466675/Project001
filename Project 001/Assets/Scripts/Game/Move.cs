using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public class Move : MonoBehaviour
	{
        public Actor actor;

        private void FixedUpdate()
        {
            float x, y;
            bool isRun = Input.GetKey(KeyCode.LeftShift);

            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
 
            if (x != 0)
            {
                if (x > 0)
                {
                    if (isRun)
                    {
                        actor.PlayAction("RunRight");
                    }
                    else
                    {                        
                        actor.PlayAction("WalkRight");
                    }
                }
                else
                {
                    if (isRun)
                    {
                        actor.PlayAction("RunLeft");
                    }
                    else
                    {
                        actor.PlayAction("WalkLeft");
                    }                    
                }
            }
            else if(y != 0)      
            {
                if(y > 0) 
                {
                    if (isRun)
                    {
                        actor.PlayAction("RunUp");
                    }
                    else
                    {
                        actor.PlayAction("WalkUp");
                    }                    
                }
                else
                {
                    if (isRun)
                    {
                        actor.PlayAction("RunDown");
                    }
                    else
                    {
                        actor.PlayAction("WalkDown");
                    }                    
                }
            }
            else
            {
                actor.PlayAction("Idle");
            }
        }       
    }
}

