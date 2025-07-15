namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitComponent
    {
        private UnitBase unit;

        internal void InitComponent(UnitBase unit) 
        {
            this.unit = unit;
        }
        internal void Destroy() 
        {
            OnDestroyComponent();
            unit = null;
        }

        public T GetParent<T>() where T : UnitBase 
        {
            return unit as T;
        }

        public T GetComponent<T>() where T : UnitComponent
        {
            return unit.GetComponent<T>();
        }

        protected virtual void OnDestroyComponent() 
        {
        }
    }
}
