namespace GameFramework.Logic 
{
    public class MemberComponent : ECS.ComponentBase
    {
        public int MemberIndex { get; set; }
        public bool IsLeader { get; private set; }

        public void SetIsLeader(bool isLeader)
        {
            IsLeader = isLeader;
        }
    }
}

