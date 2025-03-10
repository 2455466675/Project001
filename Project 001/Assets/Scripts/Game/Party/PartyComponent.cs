using EC;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyComponent : EC.Component, IAwake
    {
        private List<CharacterComponent> characters;

        public void Awake()
        {
            characters = new List<CharacterComponent>();

            for (int i = 0; i < 4; i++)
            {
                Entity character = MyEntity.CreateChild();
                CharacterComponent cc = character.AddComponent<CharacterComponent>();
                cc.Init(1001);
                
                characters.Add(cc);                       
            }

            for (int i = 0; i < characters.Count; i++)
            {
                CharacterComponent cc = characters[i];
                QueueableComponent qc = cc.GetComponent<QueueableComponent>();
                ActorComponent ac = cc.GetComponent<ActorComponent>();
                if (i == 0) 
                {
                    qc.IsLeader = true;
                    qc.Next = characters[i + 1];
                    ac.SetColloderEnabled(true);
                }
                else
                {
                    qc.IsLeader = false;
                    qc.Prev = characters[i - 1];
                    qc.Next = i == characters.Count - 1 ? null : characters[i + 1];
                    ac.SetColloderEnabled(false);
                }
            }
        }

        public void Move(Vector2 dir) 
        {
            CharacterComponent cc = characters[0];
            MotorComponent motor = cc.GetComponent<MotorComponent>();
            motor.Move(dir);
        }

        public void Run(bool isRunning)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                CharacterComponent cc = characters[i];
                MotorComponent motor = cc.GetComponent<MotorComponent>();
                motor.IsRunning = isRunning;
            }
        }
    }
}
