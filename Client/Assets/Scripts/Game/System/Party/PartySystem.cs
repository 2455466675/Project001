using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Game.GSystem
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

            for (int i = 1; i <= 4; i++)
            {
                PartyUnit unit = Game.System.UnitManager.CreateUnit<PartyUnit>();
                unit.Init(810000 + i);
                units.Add(unit);

                if (i == 1) 
                {
                    this.leader = unit;
                }                
            }   
        }

        public async UniTask RefreshActor() 
        {
            for (int i = 0; i < units.Count; i++)
            {
                PartyUnit unit = units[i];
                await unit.GetComponent<ActorComponent>().RefreshActorAsync();
            }

            for (int i = 0; i < units.Count; i++)
            {
                PartyUnit unit = units[i];

                PartyComponent partyComponent = unit.GetComponent<PartyComponent>();
                partyComponent.SetIsLeader(i == 0);

                if (i > 0)
                {
                    partyComponent.SetPrev(units[i - 1].GetComponent<PartyComponent>());
                }

                if (i < units.Count - 1)
                {
                    partyComponent.SetNext(units[i + 1].GetComponent<PartyComponent>());
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
