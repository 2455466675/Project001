using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Core
{
    public struct CameraRotateArgs : IGameEventArgs
    {
        public Quaternion rotation;
    }

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
                    node.SetPosition(m_Target.position);
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
                Quaternion target = Quaternion.Euler(45f, 0f, 0f);
                node.SetRotation(target);
            }

            if (m_State == CameraState.Follow)
            {
                CameraNode node = GameRoot.GetNode<CameraNode>();
                Quaternion target = Quaternion.Euler(20f, 0f, 0f);
                node.SetRotation(target);
            }
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        public void Move(Vector3 dir)
        {
            CameraNode node = GameRoot.GetNode<CameraNode>();

            Vector3 position = node.GetPositon();
            Vector3 target = position + 2f * Time.fixedDeltaTime * dir;

            node.SetPosition(target);
        }

        public void Rotate(Vector2 dir)
        {
            CameraNode node = GameRoot.GetNode<CameraNode>();

            Quaternion rotation = node.GetRotation();
            float x = rotation.eulerAngles.x + 10f * Time.fixedDeltaTime * dir.y;
            float v = Utility.Math.Clamp(x, 20f, 60f);
            Quaternion target = Quaternion.Euler(v, 0f, 0f);

            node.SetRotation(target);

            Game.Event.Publish(new CameraRotateArgs() { rotation = target });
        }

        public void LookAt(Vector3 target) 
        {
            CameraNode node = GameRoot.GetNode<CameraNode>();
            node.SetPosition(target);

            //Test(target).Forget();
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
