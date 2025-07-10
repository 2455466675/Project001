using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem 
{
    public enum UnitDirection 
    {
        Right = 0,
        Left  = 1,
    }

    public class BattleTransformComponent : UnitComponent
    {
        public UnitDirection dir = UnitDirection.Left;

        public bool IsLeft => dir == UnitDirection.Left;

        public float speed = 1f;

        public int pX;
        public int pY;
              
        public void SetPosition(int x, int y) 
        {
            if (x < pX) 
            {
                if (dir == UnitDirection.Right) 
                {
                    dir = UnitDirection.Left;
                }
            }
            else if (x > pX)
            {
                if (dir == UnitDirection.Left)
                {
                    dir = UnitDirection.Right;
                }
            }

            pX = x; 
            pY = y;            
        }

        public void MoveByPath(List<Vector2Int> path)
        {
            MoveAsync(path).Forget();
        }

        private async UniTask MoveAsync(List<Vector2Int> path) 
        {
            ActorComponent actor = GetComponent<ActorComponent>();

            Vector2Int endPoint = path[^1];

            if (endPoint.x < pX) 
            {
                actor.PlayAction(Common.StringToHash("FollowRunLeft"));
            }
            else if (endPoint.x > pX)
            {
                actor.PlayAction(Common.StringToHash("FollowRunRight"));
            }
            else
            {
                if (IsLeft)
                {
                    actor.PlayAction(Common.StringToHash("FollowRunLeft"));
                }
                else
                {
                    actor.PlayAction(Common.StringToHash("FollowRunRight"));
                }
            }

            for (int i = 0; i < path.Count; i++)
            {
                Vector2Int point = path[i];
                Vector3 pp = BattleUtil.IndexToPosition(point.x, point.y);

                if (i == 0) 
                {
                    actor.SetLocalPosition(pp);
                }
                else
                {
                    Vector3 ap = actor.GetLocalPosition();

                    int d = GameMathf.Abs(point.x - path[i - 1].x) + GameMathf.Abs(point.y - path[i - 1].y);

                    float t = 0.2f * d;
                    float t1 = t;
                    while (t1 > 0f)
                    {
                        Vector3 r = Vector3.Lerp(ap, pp, 1 - t1 / t);
                        actor.SetLocalPosition(r);
                        await UniTask.Yield();
                        t1 -= Time.deltaTime * speed;
                    }

                    actor.SetLocalPosition(pp);
                }

                //await UniTask.Yield();
            }

            if (endPoint.x < pX)
            {                
                actor.PlayAction(Common.StringToHash("IdleLeft"));
            }
            else if (endPoint.x > pX)
            {
                actor.PlayAction(Common.StringToHash("IdleRight"));
            }
            else
            {
                if (IsLeft)
                {                    
                    actor.PlayAction(Common.StringToHash("IdleLeft"));
                }
                else
                {
                    actor.PlayAction(Common.StringToHash("IdleRight"));
                }
            }

            SetPosition(endPoint.x, endPoint.y);
        }
    }
}