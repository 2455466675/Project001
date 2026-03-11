using GameFramework.Utility.GameDefine;
using System.Collections.Generic;

namespace GameFramework.View.UI
{
    public class UIEntityManager
    {
        private Dictionary<PanelDefine, PanelEntity> panelEntities;

        public void Init()
        {
            panelEntities = new Dictionary<PanelDefine, PanelEntity>();
        }

        public void ShowPanel(PanelDefine id, object content)
        {
            PanelEntity entity = GetOrCreatePanelEntity(id);
            entity.Show(content);
        }

        public void HidePanel(PanelDefine id)
        {            
            PanelEntity entity = GetPanelEntity(id);
            if (entity != null)
            {
                entity.Hide();
            }
            else
            {
                MDebug.Error("PanelEntity is null", id.ToString());
            }
        }

        public PanelEntity GetPanelEntity(PanelDefine id)
        {
            if (panelEntities.ContainsKey(id))
            {
                return panelEntities[id];
            }
            else
            {
                return null;
            }
        }

        public NavigationListEntity GetNavigationListEntity(NavigationDefine id)
        {
            PanelDefine panelDefine = Game.GetSystem<UISystem>().GetPanelDefine(id);
            PanelEntity panelEntity = GetPanelEntity(panelDefine);
            if (panelEntity == null)
            {
                return null;
            }
            else
            {
                return panelEntity.GetNavigationListEntity(id);
            }
        }

        private PanelEntity GetOrCreatePanelEntity(PanelDefine id)
        {
            PanelEntity panel = GetPanelEntity(id);
            if (panel == null)
            {
                panel = new PanelEntity();
                panel.Init(id);
                panelEntities.Add(id, panel);
            }
            return panel;
        }
    }
}