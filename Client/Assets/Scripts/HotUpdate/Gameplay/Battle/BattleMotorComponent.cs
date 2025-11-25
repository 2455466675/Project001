using Cysharp.Threading.Tasks;
using GameFramework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleMotorComponent : Featrue.Component, IFixedUpdate
    {
        private StateMachine m_Machine;

        private bool isStartUp;

        protected override void OnInit()
        {
            m_Machine = new StateMachine();

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
            SetXY(-1f, 0f);
            SetXY(0f, 0f);
            m_Machine.Run<IdleState>();
            isStartUp = true;
        }

        private void SetXY(float x, float y)
        {
            m_Machine.SetBlackboardValue("LastDirX", m_Machine.GetBlackboardFloatValue("DirX"));
            m_Machine.SetBlackboardValue("LastDirY", m_Machine.GetBlackboardFloatValue("DirY"));
            m_Machine.SetBlackboardValue("DirX", x);
            m_Machine.SetBlackboardValue("DirY", y);
        }

        public async UniTask MoveAsync(List<Vector2Int> path)
        {
            ActorComponent actor = GetComponent<ActorComponent>();

            float dirX = m_Machine.GetBlackboardFloatValue("LastDirX");

            for (int i = 0; i < path.Count; i++)
            {
                Vector2Int point = path[i];
                Vector3 pp = Game.GetSystem<BattleSystem>().GridManager.Coord2Pos(point.x, point.y);

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

                    SetXY(dirX, 0f);

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

            SetXY(0f, 0f);

            Vector2Int endPoint = path[^1];
            GetComponent<BattleTransformComponent>().SetCoordPosition(endPoint.x, endPoint.y);
        }
    }
}
