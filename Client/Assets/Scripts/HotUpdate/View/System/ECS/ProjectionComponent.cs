namespace GameFramework.View
{
    /// <summary>
    /// 视图层投影组件
    /// </summary>
    public class ProjectionComponent : ECS.ComponentBase
    {
        /// <summary>
        /// 逻辑层本体Eid
        /// </summary>
        public int HostEid { get; private set; }

        /// <summary>
        /// 关联逻辑层本体
        /// </summary>
        /// <param name="hostEid"></param>
        public void SetHost(int hostEid)
        {
            this.HostEid = hostEid;
        }
    }
}