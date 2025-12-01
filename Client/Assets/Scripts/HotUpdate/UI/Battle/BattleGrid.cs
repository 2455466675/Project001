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
                Game.GetSystem<BattleViewSystem>().OnEnterBattle();
            }
            else
            {             
                Game.GetSystem<BattleViewSystem>().OnEixtBattle();
            }
        }
    }

    public class BattleGrid : MonoBehaviour
    {
        [SerializeField]
        private NavigationView m_NavigationView;
        [SerializeField]
        private BattleTileLayout m_TileLayout;

        public NavigationView NavigationView => m_NavigationView;
        public int RowCount => m_TileLayout != null ? m_TileLayout.RowCount : 0;
        public int ColCount => m_TileLayout != null ? m_TileLayout.ColCount : 0;

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
