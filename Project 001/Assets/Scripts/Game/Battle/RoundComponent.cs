using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    public class BattlePhase 
    {
        public virtual async UniTask<bool> Execute()
        {
            await UniTask.Yield();
            return true;
        }
    }

    public class BattlePreprocess : BattlePhase
    {
        
    }

    public class BattleStart : BattlePhase
    {

    }

    public class RoundStart : BattlePhase
    {

    }

    public class UnitAction : BattlePhase 
    {
    
    }

    public class RoundEnd : BattlePhase
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class RoundComponent : EC.Component
    {
        private BattlePreprocess battlePreprocess;
        private BattleStart battleStart;
        private RoundStart roundStart;
        private UnitAction unitAction;
        private RoundEnd roundEnd;

        public async void Start() 
        {
            await battlePreprocess.Execute();

            await battleStart.Execute();

            bool r;

            while (true) 
            {
                r = await roundStart.Execute();
                if (!r) 
                {
                    break;
                }

                r = await unitAction.Execute();
                if (!r)
                {
                    break;
                }

                r = await roundEnd.Execute();
                if (!r)
                {
                    break;
                }
            }
        }
    }
}
