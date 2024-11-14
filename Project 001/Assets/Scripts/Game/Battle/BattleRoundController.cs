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
        private List<BattleRole> characters;

        private List<BattleStepBaseState> states;

        private BattleStepBaseState currentState;   

        public BattleRoundController()
        {

            states = new List<BattleStepBaseState>
            {
                new BattlePrepState(this),
                new BattleStartState(this),
                new RoundStartState(this),
                new CharacterActionState(this),
                new RoundEndState(this)
            };
        }

        public void SwitchState(BattleStep Step)
        {
            BattleStepBaseState state = states.Find(s => s.Step == Step);

            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
        }

        public void ActionDetermine(object data)
        {
            BattleStepBaseState state = states.Find(s => s.Step == BattleStep.CharacterAction);
            (state as CharacterActionState).ActionDetermine(data);
        }

        public void StartFight(List<BattleRole> characters)
        {
            this.characters = characters;

            SwitchState(BattleStep.BattlePrep);            
        }

        public bool IsOver()
        {
            return false;
        }

        public void RandomList()
        {
            for(int i = 0; i < characters.Count; i++)
            {
                int index1 = Random.Range(0, characters.Count);
                int index2 = Random.Range(0, characters.Count);
                while (index1 == index2)
                {
                    index2 = Random.Range(0, characters.Count);
                }

                (characters[index2], characters[index1]) = (characters[index1], characters[index2]);
            }
        }

        private int index = 0;

        public BattleRole GetCharacter()
        {
            if (index >= characters.Count)
            {
                RandomList();
                index = 0;
            }

            return characters[index++];
        }
    }
}

