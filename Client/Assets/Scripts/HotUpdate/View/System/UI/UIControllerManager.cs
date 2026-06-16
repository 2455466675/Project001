using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class UIControllerManager
    {
        private Dictionary<PanelDefine, IPanelController> panelControllers;
        private Dictionary<NavigationDefine, INavigationController> navigationControllers;
        private Dictionary<PanelDefine, NavigationDefine[]> panelNavigationMap;
        private Dictionary<NavigationDefine, PanelDefine> navigationPanelMap;

        public void Init()
        {
            panelControllers = new Dictionary<PanelDefine, IPanelController>();
            navigationControllers = new Dictionary<NavigationDefine, INavigationController>();
            panelNavigationMap = new Dictionary<PanelDefine, NavigationDefine[]>();
            navigationPanelMap = new Dictionary<NavigationDefine, PanelDefine>();

            var navigationControllerTypes = Game.GetTypes<NavigationControllerAttribute>();
            for (int i = 0; i < navigationControllerTypes.Length; i++)
            {
                Type type = navigationControllerTypes[i].type;
                if (typeof(INavigationController).IsAssignableFrom(type))
                {
                    NavigationControllerAttribute attribute = type.GetCustomAttribute(typeof(NavigationControllerAttribute), false) as NavigationControllerAttribute;
                    INavigationController controller = Activator.CreateInstance(type) as INavigationController;
                    navigationControllers.Add(attribute.Id, controller);
                }
            }

            var panelControllerTypes = Game.GetTypes<UIPanelControllerAttribute>();
            for (int i = 0; i < panelControllerTypes.Length; i++)
            {
                Type type = panelControllerTypes[i].type;
                if (typeof(IPanelController).IsAssignableFrom(type))
                {
                    UIPanelControllerAttribute attribute = type.GetCustomAttribute(typeof(UIPanelControllerAttribute), false) as UIPanelControllerAttribute;
                    IPanelController controller = Activator.CreateInstance(type) as IPanelController;

                    int childCount = attribute.Children.Length;
                    NavigationDefine[] children = new NavigationDefine[childCount];
                    for (int j = 0; j < childCount; j++)
                    {
                        NavigationDefine navigationDefine = attribute.Children[j];
                        navigationPanelMap[navigationDefine] = attribute.Id;
                        children[j] = navigationDefine;
                    }

                    panelNavigationMap.Add(attribute.Id, children);
                    panelControllers.Add(attribute.Id, controller);
                }
            }
        }

        public IPanelController GetPanelController(PanelDefine id)
        {
            if (panelControllers.ContainsKey(id))
            {
                return panelControllers[id];
            }
            else
            {
                return null;
            }
        }

        public INavigationController GetNavigationController(NavigationDefine id)
        {
            if (navigationControllers.ContainsKey(id))
            {
                return navigationControllers[id];
            }
            else
            {
                return null;
            }
        }

        public NavigationDefine[] GetNavigationDefines(PanelDefine id)
        {
            if (panelNavigationMap.ContainsKey(id))
            {
                return panelNavigationMap[id];
            }
            else
            {
                return Array.Empty<NavigationDefine>();
            }
        }

        public PanelDefine GetPanelDefine(NavigationDefine id)
        {
            if (navigationPanelMap.ContainsKey(id))
            {
                return navigationPanelMap[id];
            }
            else
            {
                return PanelDefine.None;
            }
        }
    }

}