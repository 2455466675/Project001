using GameFramework.Core;
using GameFramework.Gameplay;
using UnityEngine;

namespace GameFramework.UI
{
    [GameEvent]
    public class EnterBattleEvent_Handler : GameEventHandlerBase<EnterBattleEventArgs>
    {
        public override void Invoke(EnterBattleEventArgs arg)
        {
            bool isEnter = arg.isEnter;
            if (isEnter)
            {
                GameRoot.GetNode<ActorNode>().ChangeBattleModel(true);

                var battleNode = GameRoot.GetNode<BattleNode>();
                battleNode.LoadGrid(0, 0);

                BattleGridController controller = new BattleGridController();
                controller.Show(BattleGrid.Instance.NavigationView);

                BattleGridCammand cammand = new BattleGridCammand(controller, new int[] { 0 });
                Game.GetModule<InputController>().PushCammand(cammand);
            }
            else
            {
                var battleNode = GameRoot.GetNode<BattleNode>();
                battleNode.DestroyGrid();

                GameRoot.GetNode<ActorNode>().ChangeBattleModel(false);
            }
        }
    }

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

    public class BattleGrid : MonoBehaviour
    {
        [SerializeField]
        private NavigationView m_NavigationView;

        [SerializeField]
        private BattleTileLayout m_TileLayout;

        public int RowCount => m_TileLayout != null ? m_TileLayout.RowCount : 0;
        public int ColCount => m_TileLayout != null ? m_TileLayout.ColCount : 0;

        public static BattleGrid Instance { get; private set; }

        public NavigationView NavigationView => m_NavigationView;

        private void Awake()
        {
            Instance = this;
        }

        public NavigationItemView GetTileItem(int index)
        {
            if (m_NavigationView == null)
            {
                return null;
            }
            return m_NavigationView.GetNavigationItemView(index);
        }

        public NavigationItemView GetTileItem(int x, int y)
        {
            if (m_NavigationView == null)
            {
                return null;
            }

            int index = Pos2Index(x, y);
            return m_NavigationView.GetNavigationItemView(index);
        }

        public int Pos2Index(int x, int y)
        {
            return y * RowCount + x;
        }

        public Vector2Int Index2Pos(int index)
        {
            int x = index % ColCount;
            int y = index / ColCount;
            return new Vector2Int(x, y);
        }
    }
}
