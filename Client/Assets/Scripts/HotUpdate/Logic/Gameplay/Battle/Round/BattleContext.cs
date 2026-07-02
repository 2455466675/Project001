using UnityEngine;

namespace GameFramework.Logic
{
    public interface IActionSchedule
    {
        void Rebuild();

        bool Dequeue(out int battleID);
    }

    public interface IBattleContext
    {
        IActionSchedule Schedule { get; }

        bool CheckFinish();
    }

    public class BattleContext : IBattleContext
    {
        public IActionSchedule Schedule { get; private set; }

        public BattleContext()
        {
            Schedule = new ActionSchedule();
        }

        public bool CheckFinish()
        {
            return false;
        }
    }

    public class ActionSchedule : IActionSchedule
    {
        public bool Dequeue(out int battleID)
        {
            throw new System.NotImplementedException();
        }

        public void Rebuild()
        {
            throw new System.NotImplementedException();
        }
    }
}
