using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleRoundController
	{
        private List<BattleRole> roles;

        private List<BattleStageBase> states;

        private BattleStageBase currentState;   

        public BattleRoundController()
        {

            states = new List<BattleStageBase>
            {
                new BattlePrepStage(this),
                new BattleStartStage(this),
                new RoundStartStage(this),
                new RoleActionStage(this),
                new RoundEndStage(this)
            };
        }

        public void SwitchState(BattleStage Step)
        {
            BattleStageBase state = states.Find(s => s.Stage == Step);

            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
        }

        public void ActionDetermine(object data)
        {
            BattleStageBase state = states.Find(s => s.Stage == BattleStage.CharacterAction);
            (state as RoleActionStage).ActionDetermine(data);
        }

        public void StartFight(List<BattleRole> roles)
        {
            this.roles = roles;

            SwitchState(BattleStage.BattlePrep);            
        }

        public bool IsOver()
        {
            return false;
        }

        public void RandomList()
        {
            for(int i = 0; i < roles.Count; i++)
            {
                int index1 = Random.Range(0, roles.Count);
                int index2 = Random.Range(0, roles.Count);
                while (index1 == index2)
                {
                    index2 = Random.Range(0, roles.Count);
                }

                (roles[index2], roles[index1]) = (roles[index1], roles[index2]);
            }
        }

        private int index = 0;

        public BattleRole GetCharacter()
        {
            if (index >= roles.Count)
            {
                RandomList();
                index = 0;
            }

            return roles[index++];
        }
    }
}

