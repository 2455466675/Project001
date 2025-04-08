using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class NavigationProxyManager
    {
        private static Dictionary<NavigationGroupDefine, NavigationGroupProxy> groupProxys;
        private static Dictionary<NavigationListDefine, NavigationListProxy> listProxys;

        public static void Init()
        {
            groupProxys = new Dictionary<NavigationGroupDefine, NavigationGroupProxy>();
            listProxys = new Dictionary<NavigationListDefine, NavigationListProxy>();

            List<Type> groupProxyTypes = Game.Code.GetTypes<GroupProxyAttribute>();
            List<Type> listProxyTypes = Game.Code.GetTypes<ListProxyAttribute>();

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

        public static NavigationGroupProxy GetNavigationGroupProxy(NavigationGroupDefine define) 
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

        public static NavigationListProxy GetNavigationListProxy(NavigationListDefine define)
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
