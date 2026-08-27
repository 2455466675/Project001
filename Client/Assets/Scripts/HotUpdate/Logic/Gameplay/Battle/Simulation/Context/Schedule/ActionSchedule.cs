using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IActionSchedule
    {
        void Rebuild(IBattleContext context);

        bool Dequeue(out int battleID);
    }

    public class ActionSchedule : IActionSchedule
    {
        private readonly List<int> schedule;

        public ActionSchedule()
        {
            schedule = new List<int>();
        }

        public bool Dequeue(out int battleID)
        {
            if (schedule.Count == 0)
            {
                battleID = -1;
                return false;
            }
            else
            {
                battleID = schedule[0];
                schedule.RemoveAt(0);
                return true;
            }
        }

        public void Rebuild(IBattleContext context)
        {
            schedule.Clear();
            schedule.Add(1);
            schedule.Add(2);
            schedule.Add(3);
            schedule.Add(4);
        }
    }

}
