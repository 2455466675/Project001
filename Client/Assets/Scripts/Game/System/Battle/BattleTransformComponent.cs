using Cysharp.Threading.Tasks;
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

            UnitDirection direction = this.dir;

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

                    Vector2Int beforPoint = path[i - 1];

                    int j = i;

                    while (j <= path.Count - 1) 
                    {
                        Vector2Int p = path[j];

                        if (p.x > beforPoint.x)
                        {
                            direction = UnitDirection.Right;
                            break;
                        }
                        else if (p.x < beforPoint.x)
                        {
                            direction = UnitDirection.Left;
                            break;
                        }
                        else
                        {
                            j++;
                        }
                    }
                    
                    if (direction == UnitDirection.Right)
                    {
                        actor.PlayAction(Common.StringToHash("AnimatorRunRight"));
                    }
                    else if (direction == UnitDirection.Left)
                    {
                        actor.PlayAction(Common.StringToHash("AnimatorRunLeft"));
                    }

                    int d = GameMathf.Abs(point.x - beforPoint.x) + GameMathf.Abs(point.y - beforPoint.y);

                    float t = 0.2f * d;
                    float t1 = t;
                    while (t1 > 0f)
                    {
                        t1 -= Time.deltaTime * speed;
                        Vector3 r = Vector3.Lerp(ap, pp, 1 - t1 / t);
                        actor.SetLocalPosition(r);
                        await UniTask.Yield();
                    }

                    actor.SetLocalPosition(pp);
                }

                //await UniTask.Yield();
            }

            if (direction == UnitDirection.Right)
            {
                actor.PlayAction(Common.StringToHash("AnimatorIdleRight"));
            }
            else if (direction == UnitDirection.Left)
            {
                actor.PlayAction(Common.StringToHash("AnimatorIdleLeft"));
            }

            Vector2Int endPoint = path[^1];
            SetPosition(endPoint.x, endPoint.y);
            this.dir = direction;
        }
    }
}