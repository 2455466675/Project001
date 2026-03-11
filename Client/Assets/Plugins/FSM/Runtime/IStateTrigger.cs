namespace FSM
{
    public interface IStateTrigger
    {
        public bool Check(IBlackboard blackboard);
    }
}