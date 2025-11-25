using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Core
{
    public enum CameraState
    {
        Follow,
        Battle,
    }

    [GameModule]
    public class CameraManager : IGameModule_SyncInit, ILateUpdate
    {
        private Transform m_Target;

        private CameraState m_State;

        private Vector3 m_TargetPos;

        public void Init()
        {
            m_State = CameraState.Follow;
        }

        public void LateUpdate()
        {
            if (m_State == CameraState.Follow)
            {
                if (m_Target != null)
                {
                    CameraNode node = GameRoot.GetNode<CameraNode>();
                    node.transform.position = new Vector3(m_Target.position.x, m_Target.position.y + 2, m_Target.position.z - 6);
                }
            }

            if (m_State == CameraState.Battle)
            {

            }
        }

        public void SetState(CameraState state)
        {
            this.m_State = state;
            if (m_State == CameraState.Battle)
            {
                CameraNode node = GameRoot.GetNode<CameraNode>();
                node.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
            }

            if (m_State == CameraState.Follow)
            {
                CameraNode node = GameRoot.GetNode<CameraNode>();
                node.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
            }
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        public void Move(Vector3 dir)
        {
            CameraNode node = GameRoot.GetNode<CameraNode>();
            node.transform.position += 2.5f * Time.fixedDeltaTime * dir;
        }

        public void LookAt(Vector3 target) 
        {
            Test(target).Forget();
        }
        
        private async UniTaskVoid Test(Vector3 target)
        {
            target += new Vector3(0, 4, -4);

            m_TargetPos = target;

            CameraNode node = GameRoot.GetNode<CameraNode>();
            float t = 5f;
            float t1 = 0f;
            while (t1 < t)
            {
                node.transform.position = Vector3.Lerp(node.transform.position, m_TargetPos, t1 / t);
                t1 += Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
}
