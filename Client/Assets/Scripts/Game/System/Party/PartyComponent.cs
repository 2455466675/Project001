
namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyComponent : UnitComponent
    {
        public bool IsLeader { get; private set; }

        public PartyComponent Prev { get; private set; }
        public PartyComponent Next { get; private set; }

        public void SetIsLeader(bool isLeader)
        {
            this.IsLeader = isLeader;

            GetComponent<ActorComponent>().SetRigidbodyEnable(isLeader);
        }

        public void SetPrev(PartyComponent prev) 
        {
            this.Prev = prev;
        }

        public void SetNext(PartyComponent next) 
        {
            this.Next = next;
        }

        public void TransmitTrace(MoveTrace trace)
        {
            if (Next == null)
            {
                return;
            }
            MotorComponent motor = Next.GetComponent<MotorComponent>();
            motor.PushTrace(trace);
        }
    }
}
