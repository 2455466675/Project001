using System;
using System.Collections.Generic;
using GameFramework.Featrue;
using GameFramework.Core;
using System.Reflection;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFramework.UI 
{
    [GameModule(GameModulePriority.UIManager)]
    public class UIManager : IGameModule_SyncInit
    {
        private Dictionary<PanelDefine, PanelController> m_PanelControllers;
        private Dictionary<NavigationDefine, NavigationController> m_NavigationControllers;
        private Dictionary<PanelDefine, NavigationDefine[]> m_PanelMap;
        private Dictionary<NavigationDefine, PanelDefine> m_NavigationMap;

        private Dictionary<PanelDefine, Entity> m_Panels;
        private Dictionary<NavigationDefine, Entity> m_Navigations;

        public void Init()
        {
            m_Panels = new Dictionary<PanelDefine, Entity>();
            m_Navigations = new Dictionary<NavigationDefine, Entity>();
            InitControllers();
        }

        public void ShowPanel(PanelDefine id, object content = null)
        {
            Entity entity;
            if (m_Panels.ContainsKey(id) ) 
            {
                entity = m_Panels[id];
            }
            else
            {
                entity = InitPanel(id);
            }
            entity.GetComponent<PanelComponent>().Show(content);

            var children = m_PanelMap[id];
            if (children != null) 
            {
                for (int i = 0; i < children.Length; i++)
                {
                    var child = GetListEntity(children[i]);
                    child?.GetComponent<NavigationListComponent>().Show();
                }
            }           
        }

        public void HidePanel(PanelDefine id)
        {
            if (!m_Panels.ContainsKey(id))
            {
                return;
            }
            Entity entity = m_Panels[id];
            entity.GetComponent<PanelComponent>().Hide();

            var children = m_PanelMap[id];
            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    var child = GetListEntity(children[i]);
                    child?.GetComponent<NavigationListComponent>().Hide();
                }
            }
        }

        private Entity InitPanel(PanelDefine id)
        {
            var panelEntity = Game.GetModule<EntityManager>().CreateEntity();
            var pc = panelEntity.AddComponent<PanelComponent>();

            var panelControll = GetPanelController(id);
            pc.Init(id, panelControll);

            var views = pc.GetNavigationViews();
            var children = m_PanelMap[id];
            var length1 = children != null ? children.Length : 0;
            var length2 = views != null ? views.Length : 0;
            int length3 = Utility.Math.Min(length1, length2);
            for (int i = 0; i < length3; i++)
            {
                var listEntity = Game.GetModule<EntityManager>().CreateEntity();
                var nlc = listEntity.AddComponent<NavigationListComponent>();
                var navigationController = GetNavigationController(children[i]);
                nlc.Init(views[i], navigationController);
                m_Navigations.Add(children[i], listEntity);
            }

            m_Panels.Add(id, panelEntity);
            return panelEntity;
        }

        public void Navigate(NavigationDefine id, int[] defaultIndexs = null)
        {
            MDebug.Log("Navigate");
            if (!m_NavigationMap.ContainsKey(id)) 
            {
                MDebug.Log("Navigate 2");
                return;
            }

            defaultIndexs ??= new int[] {0};

            PanelDefine panel = m_NavigationMap[id];
            Game.GetModule<InputController>().Navigate(id, panel, defaultIndexs);
        }

        public Entity GetPanelEntity(PanelDefine id)
        {
            return m_Panels[id];
        }

        public Entity GetListEntity(NavigationDefine id)
        {
            return m_Navigations[id];
        }

        public PanelController GetPanelController(PanelDefine id)
        {
            if (m_PanelControllers.ContainsKey(id)) 
            {
                return m_PanelControllers[id];
            }
            else
            {
                return null;
            }
        }

        public NavigationController GetNavigationController(NavigationDefine id)
        {
            if (m_NavigationControllers.ContainsKey(id))
            {
                return m_NavigationControllers[id];
            }
            else
            {
                return null;
            }
        }

        private void InitControllers() 
        {
            m_PanelControllers = new Dictionary<PanelDefine, PanelController>();
            m_NavigationControllers = new Dictionary<NavigationDefine, NavigationController>();
            m_NavigationMap = new Dictionary<NavigationDefine, PanelDefine>();
            m_PanelMap = new Dictionary<PanelDefine, NavigationDefine[]>();

            Type[] types1 = Game.GetTypes<NavigationControllerAttribute>();
            for (int i = 0; i < types1.Length; i++)
            {
                Type type = types1[i];
                if (type.IsSubclassOf(typeof(NavigationController))) 
                {
                    NavigationControllerAttribute attribute = type.GetCustomAttribute(typeof(NavigationControllerAttribute), false) as NavigationControllerAttribute;
                    NavigationController controller = Activator.CreateInstance(type) as NavigationController;

                    m_NavigationControllers.Add(attribute.Id, controller);
                }
            }

            Type[] types2 = Game.GetTypes<UIPanelControllerAttribute>();
            for (int i = 0; i < types2.Length; i++)
            {
                Type type = types2[i];
                if (type.IsSubclassOf(typeof(PanelController))) 
                {
                    UIPanelControllerAttribute attribute = type.GetCustomAttribute(typeof(UIPanelControllerAttribute), false) as UIPanelControllerAttribute;
                    PanelController controller = Activator.CreateInstance(type) as PanelController;

                    int childCount = attribute.Children.Length;
                    NavigationDefine[] children = new NavigationDefine[childCount];
                    for (int j = 0; j < childCount; j++)
                    {
                        NavigationDefine navigationDefine = attribute.Children[j];
                        m_NavigationMap[navigationDefine] = attribute.Id;
                        children[j] = navigationDefine;
                    }

                    m_PanelMap.Add(attribute.Id, children);
                    m_PanelControllers.Add(attribute.Id, controller);
                }
            }
        }

    }
}