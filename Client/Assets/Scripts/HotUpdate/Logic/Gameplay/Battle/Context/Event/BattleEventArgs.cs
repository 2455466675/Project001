namespace GameFramework.Logic
{
    public class BattleEventArgs
    {
        public int SourceID { get; set; }
        public int TargetID { get; set; }
        public int Value { get; set; }
        public bool Cancel { get; set; }
    }
}
