namespace Navigation
{
    public interface INavigation
    {
        public void Move(float h, float v);
        public void Submit();
        public bool InFocus(bool isRefocus, int[] indexs = null);
        public void OutFocus();
        public void Exit();
    }
}