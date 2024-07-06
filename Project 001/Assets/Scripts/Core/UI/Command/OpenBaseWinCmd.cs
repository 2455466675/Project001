using Game.Core;

namespace Game.UI
{
    public class OpenBaseWinCmd : ICommand
    {
        public bool Undoable => id != 100002;

        private readonly int id;

        public OpenBaseWinCmd(int id)
        {
            this.id = id;
        }

        public bool Execute()
        {
            if (GameCore.StateController.IsUIModel)
            {
                MLog.Warn("当前已是UI模式：" + id);
                return false;
            }
            GameCore.StateController.SwitchModel(GameModel.UI);
            return true;
        }

        public bool Undo()
        {
            if (!Undoable)
            {
                return false;
            }
            GameCore.StateController.SwitchModel(GameModel.SCENE);
            return true;
        }
    }
}
