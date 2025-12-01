using GameFramework.Core;
using GameFramework.Gameplay;
using UnityEngine;

namespace GameFramework.UI
{
    public class BattleGridCammand : InputCammand
    {
        private NavigationController controller;
        private int[] defaultIndexs;

        public BattleGridCammand(NavigationController controller, int[] defaultIndexs)
        {
            this.controller = controller;
            this.defaultIndexs = defaultIndexs;
        }

        protected override void OnPop()
        {
            controller?.Exit();
        }

        protected override bool OnPush()
        {
            if (controller == null)
            {
                return false;
            }
            else
            {
                return controller.InFocus(false, defaultIndexs);
            }
        }

        protected override bool CheckLocked()
        {
            return controller != null && controller.IsLocked;
        }

        protected override void OnInputAction(InputContext context)
        {
            InputDefine inputType = context.Input;
            switch (inputType)
            {
                case InputDefine.Move:
                    float x = context.X;
                    float y = context.Y;
                    controller?.Move(x, y);
                    break;
                case InputDefine.Submit:
                    controller?.Submit();
                    break;
            }
        }
    }

    [Gameplay]
    public class BattleViewSystem : IGameplaySystem
    {
        private BattleGrid m_BattleGrid;
        private BattleGridController m_Controller;

        public void OnInit()
        {

        }

        public void OnExit()
        {

        }

        public void OnEnterBattle()
        {
            GameRoot.GetNode<ActorNode>().ChangeBattleModel(true);

            var battleNode = GameRoot.GetNode<BattleNode>();
            m_BattleGrid = battleNode.LoadGrid<BattleGrid>(0, 0);

            m_Controller = new BattleGridController();
            m_Controller.Show(m_BattleGrid.NavigationView);
        }

        public void OnEixtBattle()
        {
            var battleNode = GameRoot.GetNode<BattleNode>();
            battleNode.DestroyGrid();

            GameRoot.GetNode<ActorNode>().ChangeBattleModel(false);

            m_BattleGrid = null;
            m_Controller = null;
        }

        public void SelectGrid(int index)
        {
            BattleGridCammand cammand = new BattleGridCammand(m_Controller, new int[] { index });
            Game.GetModule<InputController>().PushCammand(cammand);
        }
    }
}
