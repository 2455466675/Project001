using ECS;
using UnityEngine;

namespace Test
{

    //public class EntityTest : Entity, IAwake, IUpdate
    //{
    //    public void Awake()
    //    {
    //        //Debug.Log("EntityTest Awake:" + Guid);
    //    }

    //    public void Update(float dt)
    //    {
    //        //Debug.Log("EntityTest Update:" + Guid);
    //    }
    //}

    //public class ComponentTest : ECS.Component, IAwake, IUpdate
    //{
    //    public void Awake()
    //    {
    //        //Debug.Log("ComponentTest Awake:" + Guid);
    //    }

    //    public void Update(float dt)
    //    {
    //        //Debug.Log("ComponentTest Update:" + Guid);
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public class ECS_Test : MonoBehaviour
    {
        private void Start()
        {
            //EntityTest root = EntityFactory.Instance.CreateEntity<EntityTest>(-1);
            //root.AddComponent<ComponentTest>();

            //EntityTest e1 = root.CreateChild<EntityTest>();
            //e1.AddComponent<ComponentTest>();
        }

        private void Update()
        {
            //EntityFactory.Instance.Update(Time.deltaTime);
        }
    }
}
