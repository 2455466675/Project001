using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework.Featrue;
using GameFramework.Core;
using System.Reflection;

namespace GameFramework.UI 
{
    public class UIManager : IGameModule
    {
        public GameModulePriority Priority => GameModulePriority.UIManager;

        private Dictionary<PanelDefine, PanelController> m_PanelControllers;
        private Dictionary<NavigationDefine, NavigationController> m_NavigationControllers;
        private Dictionary<NavigationDefine, PanelDefine> m_NavigationMap;

        private Dictionary<PanelDefine, Entity> m_Panels;


        public async UniTask Init()
        {
            m_Panels = new Dictionary<PanelDefine, Entity>();
            InitControllers();

            await UniTask.Yield();
        }

        public void ShowPanel(PanelDefine id, object content = null)
        {
            Entity entity;
            PanelComponent pc;
            if (m_Panels.ContainsKey(id) ) 
            {
                entity = m_Panels[id];
                pc = entity.AddComponent<PanelComponent>();
                pc.Show(content);
            }
            else
            {
                entity = Game.GetModule<EntityManager>().CreateEntity();
                pc = entity.AddComponent<PanelComponent>();
                pc.SetId(id);
                pc.Load(GetPanelController(id));
                pc.Show(content);
                m_Panels.Add(id, entity);
            }
        }

        public void Navigate(NavigationDefine id)
        {
            if (!m_NavigationMap.ContainsKey(id)) 
            {
                return;
            }
            PanelDefine panel = m_NavigationMap[id];
        }

        private PanelController GetPanelController(PanelDefine id)
        {
            return m_PanelControllers[id];
        }

        private NavigationController GetNavigationController(NavigationDefine id)
        {
            return m_NavigationControllers[id];
        }

        private void InitControllers() 
        {
            m_PanelControllers = new Dictionary<PanelDefine, PanelController>();
            m_NavigationControllers = new Dictionary<NavigationDefine, NavigationController>();
            m_NavigationMap = new Dictionary<NavigationDefine, PanelDefine>();

            AssemblyManager assemblyManager = Game.GetModule<AssemblyManager>();

            Type[] types1 = assemblyManager.GetTypes<NavigationControllerAttribute>();
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

            Type[] types2 = assemblyManager.GetTypes<UIPanelControllerAttribute>();
            for (int i = 0; i < types2.Length; i++)
            {
                Type type = types2[i];
                if (type.IsSubclassOf(typeof(PanelController))) 
                {
                    UIPanelControllerAttribute attribute = type.GetCustomAttribute(typeof(UIPanelControllerAttribute), false) as UIPanelControllerAttribute;
                    PanelController controller = Activator.CreateInstance(type) as PanelController;

                    int childCount = attribute.Children.Length;
                    NavigationController[] controllers = new NavigationController[childCount];
                    for (int j = 0; j < childCount; j++)
                    {
                        NavigationDefine navigationDefine = attribute.Children[j];
                        m_NavigationMap[navigationDefine] = attribute.Id;
                        controllers[j] = GetNavigationController(navigationDefine);
                    }
                    controller.SetNavigationControllers(controllers);

                    m_PanelControllers.Add(attribute.Id, controller);
                }
            }
        }
    }
}