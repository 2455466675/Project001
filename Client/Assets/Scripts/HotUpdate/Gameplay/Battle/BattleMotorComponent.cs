using Cysharp.Threading.Tasks;
using GameFramework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleMotorComponent : Featrue.Component, IFixedUpdate
    {
        private GameStateMachine m_Machine;

        private bool isStartUp;

        protected override void OnInit()
        {
            m_Machine = new GameStateMachine();

            IdleState idleState = new IdleState(this);
            idleState.AddTrigger(new Idle2RunTirgger());
            m_Machine.AddState(idleState);

            RunState runState = new RunState(this);
            runState.AddTrigger(new Run2IdleTirgger());
            m_Machine.AddState(runState);
        }

        public void FixedUpdate()
        {
            if (!isStartUp)
            {
                return;
            }

            m_Machine.Tick();
        }

        public void StartUp()
        {
            GetComponent<ActorComponent>().SetAnimatorController(ActorAnimator.Battle);
            GetComponent<TransformComponent>().SetDirection(-1f, 0f);
            m_Machine.Run<IdleState>();
            isStartUp = true;
        }

        private void SetVelocity(float x, float y)
        {
            if (x != 0f || y != 0f)
            {
                var tc = GetComponent<TransformComponent>();
                tc.SetDirection(x, y);
            }
            m_Machine.SetBlackboardValue(DataKey.VelocityX, x);
            m_Machine.SetBlackboardValue(DataKey.VelocityY, y);
        }

        public async UniTask MoveAsync(List<Vector2Int> path)
        {
            if (path == null || path.Count <= 1)
            {
                return;
            }

            ActorComponent actor = GetComponent<ActorComponent>();

            float dirX = GetComponent<TransformComponent>().DirX;

            for (int i = 0; i < path.Count; i++)
            {
                Vector2Int point = path[i];
                Vector3 pp = BattleUtils.Coord2Pos(point.x, point.y);

                if (i == 0)
                {
                    actor.Position = pp;
                }
                else
                {
                    Vector3 ap = actor.Position;

                    Vector2Int beforPoint = path[i - 1];

                    int j = i;

                    while (j <= path.Count - 1)
                    {
                        Vector2Int p = path[j];

                        if (p.x > beforPoint.x)
                        {
                            dirX = 1f;
                            break;
                        }
                        else if (p.x < beforPoint.x)
                        {
                            dirX = -1f;
                            break;
                        }
                        else
                        {
                            j++;
                        }
                    }

                    SetVelocity(dirX, 0f);

                    int d = Utility.Math.Abs(point.x - beforPoint.x) + Utility.Math.Abs(point.y - beforPoint.y);

                    float t = 0.2f * d;
                    float t1 = t;
                    while (t1 > 0f)
                    {
                        t1 -= Time.deltaTime * 1f;
                        Vector3 r = Vector3.Lerp(ap, pp, 1 - t1 / t);
                        actor.Position = r;
                        await UniTask.Yield();
                    }

                    actor.Position = pp;
                }
            }

            SetVelocity(0f, 0f);

            Vector2Int endPoint = path[^1];
            GetComponent<BattleTransformComponent>().SetCoordPosition(endPoint.x, endPoint.y);
        }
    }
}
