namespace GameFramework.Gameplay
{
    public class TransformComponent : Featrue.Component
    {
        private float dirX;
        private float dirY;
        public float DirX => dirX;
        public float DirY => dirY;

        public void SetDirection(float dirX, float dirY)
        {
            if (this.dirX != dirX || this.dirY != dirY)
            {
                ActorComponent ac = GetComponent<ActorComponent>();
                if (ac != null)
                {
                    ac.SetFloat("DirX", dirX);
                    ac.SetFloat("DirY", dirY);
                }
            }

            this.dirX = dirX;
            this.dirY = dirY;
        }
    }
}
