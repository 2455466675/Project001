using ECS;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationProxy : Entity, IAwake
    {
        private Dictionary<NavigationGroupDefine, NavigationGroupProxy> groupProxys;
        private Dictionary<NavigationListDefine, NavigationListProxy> listProxys;

        public void Awake()
        {
            groupProxys = new Dictionary<NavigationGroupDefine, NavigationGroupProxy>();
            listProxys = new Dictionary<NavigationListDefine, NavigationListProxy>();

            CodeComponent codeComponent = GameWorld.Root.GetComponent<CodeComponent>();
            List<Type> groupProxyTypes = codeComponent.GetTypes<GroupProxyAttribute>();
            List<Type> listProxyTypes = codeComponent.GetTypes<ListProxyAttribute>();

            foreach (var t in groupProxyTypes)
            {
                Attribute attribute = t.GetCustomAttribute(typeof(GroupProxyAttribute), false);
                if (attribute is GroupProxyAttribute groupProxyAttribute)
                {
                    object o = Activator.CreateInstance(t);
                    if (o is NavigationGroupProxy proxy) 
                    {
                        groupProxys[groupProxyAttribute.Define] = proxy;
                    }
                }
            }

            foreach (var t in listProxyTypes)
            {
                Attribute attribute = t.GetCustomAttribute(typeof(ListProxyAttribute), false);
                if (attribute is ListProxyAttribute listProxyAttribute)
                {
                    object o = Activator.CreateInstance(t);
                    if (o is NavigationListProxy proxy)
                    {
                        listProxys[listProxyAttribute.Define] = proxy;
                    }
                }
            }
        }

        public NavigationGroupProxy GetNavigationGroupProxy(NavigationGroupDefine define) 
        {
            if (groupProxys.ContainsKey(define)) 
            {
                return groupProxys[define];
            }
            else
            {
                NavigationGroupProxy proxy = new NavigationGroupProxy();
                groupProxys.Add(define, proxy);
                return proxy;
            }
        }

        public NavigationListProxy GetNavigationListProxy(NavigationListDefine define)
        {
            if (listProxys.ContainsKey(define))
            {
                return listProxys[define];
            }
            else
            {
                NavigationListProxy proxy = new NavigationListProxy();
                listProxys.Add(define, proxy);
                return proxy;
            }
        }
    }
}
