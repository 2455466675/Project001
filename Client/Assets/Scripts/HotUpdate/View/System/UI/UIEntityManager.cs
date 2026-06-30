using GameFramework.Utility.GameDefine;
using System.Collections.Generic;

namespace GameFramework.View.UI
{
    public class UIEntityManager
    {
        private UIControllerManager controllerManager;
        private Dictionary<PanelDefine, PanelEntity> panelEntities;

        public void Init(UIControllerManager controllerManager)
        {
            this.controllerManager = controllerManager;
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
                entity.Destroy();
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
            PanelDefine panelDefine = controllerManager.GetPanelDefine(id);
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
                panelEntities.Add(id, panel);
            }
            panel.Init(id);
            return panel;
        }
    }
}