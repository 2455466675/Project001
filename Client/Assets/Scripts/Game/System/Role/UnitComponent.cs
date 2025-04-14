namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitComponent
    {
        private RoleUnit unit;

        public void Constructor(RoleUnit unit) 
        {
            this.unit = unit;
        }

        public T GetComponent<T>() where T : UnitComponent
        {
            return unit.GetComponent<T>();
        }

        public void Destroy() 
        {
            OnDestroy();
            unit = null;
        }

        protected virtual void OnDestroy() 
        {
        }
    }
}
