using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class PartySystem
    {
        private List<PartyUnit> units;
        private PartyUnit leader;

        public void Init() 
        {
            units = new List<PartyUnit>();

            for (int i = 1; i < 3; i++)
            {
                PartyUnit unit = Game.System.RoleSystem.CreateUnit<PartyUnit>();
                unit.Init(810000 + i);
                units.Add(unit);

                if (i == 1) 
                {
                    this.leader = unit;
                }                
            }

            for (int i = 0; i < units.Count; i++)
            {
                PartyUnit unit = units[i];

                QueueableComponent queueableComponent = unit.GetComponent<QueueableComponent>();
                queueableComponent.IsLeader = i == 0;

                ActorComponent actorComponent = unit.GetComponent<ActorComponent>();
                actorComponent.SetRigidbodyEnable(i == 0);

                if (i > 0) 
                {            
                    queueableComponent.Prev = units[i - 1].GetComponent<QueueableComponent>();
                }
                
                if (i < units.Count - 1)
                {
                    queueableComponent.Next = units[i + 1].GetComponent<QueueableComponent>();
                }
            }
        }

        public void Move(float x, float y)
        {
            leader.Move(x, y);
        }

        public void Run(bool isRunning)
        {            
            for (int i = 0; i < units.Count; i++) 
            {
                PartyUnit unit = units[i];
                unit.Run(isRunning);                
            }
        }
    }
}
