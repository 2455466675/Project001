using Game.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    [Serializable]
    public class WinGroupItem 
    {
        public UIGroupEnum group;
        public Transform winGroup;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Camera UICamera;
        public WinGroupItem[] groups;

        public ListView ListView { get; private set;}
        public IGuidable LastSelectUI { get; private set; }
        public IGuidable CurrSelectUI { get; private set; }
        
        private Dictionary<UIGroupEnum, Transform> groupsMap;
        private event UnityAction<IGuidable> SelectUIEventHandler;
        private event UnityAction<IGuidable> DeselectUIEventHandler;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            UICamera = GetComponentInChildren<Camera>();

            InitWinGroup();
        }

        private void InitWinGroup()
        {
            if (groups != null && groups.Length > 0)
            {
                groupsMap = new Dictionary<UIGroupEnum, Transform>();
                for (int i = 0; i < groups.Length; i++)
                {
                    groupsMap.Add(groups[i].group, groups[i].winGroup);
                }
            }
        }

        public void AddSelectUIEventListener(UnityAction<IGuidable> action)
        {
            SelectUIEventHandler += action;
        }

        public void AddDeselectUIEventListener(UnityAction<IGuidable> action)
        {
            DeselectUIEventHandler += action;
        }

        public void Submit()
        {
            CurrSelectUI?.OnSubmit();
        }

        public void SelectUI(IGuidable guidableItem)
        {
            DeselectUI(CurrSelectUI);
            CurrSelectUI = guidableItem;
            CurrSelectUI?.OnSelect();
            SelectUIEventHandler?.Invoke(CurrSelectUI);
        }

        public void DeselectUI(IGuidable guidableItem)
        {
            LastSelectUI = guidableItem;
            LastSelectUI?.OnDeselect();
            DeselectUIEventHandler?.Invoke(LastSelectUI);
        }

        public void SelectListView(ListView listView) 
        {
            if (ListView != null)
            {
                ListView.OnDeselect();
            }
            ListView = listView;
            if (ListView != null)
            {
                ListView.OnSelect();
            }
        }

        public Window OpenWin(int id)
        {
            WindowCfg cfg = GameCore.GameCfgData.FindById<WindowCfg>(id);
            if (cfg == null)
            {
                Debug.LogError($"√ª”–¥∞ÃÂ≈‰÷√:{id}");
                return null;
            }
            GameObject prefab = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.path);
            Window prefabWindow = prefab.GetComponent<Window>();
            Transform parent = groupsMap[prefabWindow.group];
            Window winInst = Instantiate(prefabWindow, parent, false);
            winInst.SetCfg(cfg);

            return winInst;
        }
    }
}