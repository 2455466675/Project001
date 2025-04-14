namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueableComponent : UnitComponent
    {
        public bool IsLeader;

        public QueueableComponent Prev;
        public QueueableComponent Next;

        public bool TransmitTrace(MoveTrace trace)
        {
            if (Next == null)
            {
                return true;
            }
            MotorComponent motor = Next.GetComponent<MotorComponent>();
            return motor.PushTrace(trace);
        }
    }
}
