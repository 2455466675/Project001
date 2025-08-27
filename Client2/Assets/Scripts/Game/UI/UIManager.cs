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

        public async UniTask Init()
        {
            InitControllers();

            await UniTask.Yield();
        }

        public void ShowPanel(PanelDefine id, object content = null)
        {
            Entity entity = Game.GetModule<EntityFactory>().CreateEntity();
            PanelComponent pc = entity.AddComponent<PanelComponent>();
            pc.SetId(id);
            pc.Show(GetPanelController(id), content);
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

                    m_PanelControllers.Add(attribute.Id, controller);

                    for (int j = 0; j < attribute.Children.Length; j++)
                    {
                        NavigationDefine navigationDefine = attribute.Children[j];
                        m_NavigationMap[navigationDefine] = attribute.Id;
                    }
                }
            }
        }
    }
}