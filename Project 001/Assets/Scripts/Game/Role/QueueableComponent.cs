namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueableComponent : EC.Component
    {
        public bool IsLeader;

        public CharacterComponent Prev;
        public CharacterComponent Next;

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
