using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameUI : MonoBehaviour, ICore
    {

        public UIRoot UIRoot {get; private set;}
        public Camera UICamera => UIRoot != null ? UIRoot.UICamera : null;

        private Dictionary<int, Window> windows;
        private Stack<Window> openWinStack;

        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>("Assets/Bundles/UI/Window/UIRoot.prefab");
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            windows = new Dictionary<int, Window>();
            openWinStack = new Stack<Window>();
            yield return UIRoot;
        }
   
        public void AddSelectUIEventListener(UnityAction<IGuidable> action)
        {
            UIRoot.AddSelectUIEventListener(action);
        }

        public void AddDeselectUIEventListener(UnityAction<IGuidable> action)
        {
            UIRoot.AddDeselectUIEventListener(action);
        }

        public void Submit()
        {
            UIRoot.Submit();
        }

        public void SelectUI(IGuidable guidableItem)
        {
            UIRoot.SelectUI(guidableItem);
        }

        public void DeselectUI(IGuidable guidableItem)
        {
            UIRoot.DeselectUI(guidableItem);
        }

        public void OpenWinCommond(int id)
        {         
            CommandInvoker.ExecuteCommand(new OpenWindowCmd(id));
        }

        public void OpenWin(int id)
        {
            if (!windows.TryGetValue(id, out Window win))
            {
                win = UIRoot.OpenWin(id);
                if (win == null)
                {
                    return;
                }
                windows.Add(id, win);
            }
            if (win == null)
            {
                return;
            }

            if (openWinStack.TryPeek(out Window topWin))
            {
                topWin.Exit();
            } 

            win.Show();
            win.Enter();
            openWinStack.Push(win);
        }

        public void OpenWinAsync(int id, int frame)
        {
            GameCore.Co.WaitForFrames(() => OpenWin(id), frame);
        }

        public void CloseWin(int id)
        {
            Debug.Log($"CloseWin:{id}");
            if (openWinStack.Count <= 0)
            {
                return;
            }
            Window window = openWinStack.Pop();
            if (window.Id != id)
            {
                Debug.LogWarning($"要关闭的窗口不是顶层窗口：{id}");
                return;
            }
            window.Exit();
            window.Hide();

            if (openWinStack.TryPeek(out Window topWin))
            {
                topWin.Enter();
            }
        }

        public void SelectListView(ListView listView)
        {
            UIRoot.SelectListView(listView);
        }

        public void TestH(float h)
        {
            if (openWinStack.Count <= 0)
            {
                return;
            }
            if (UIRoot.ListView == null)
            {
                return;
            }
            if (h < 0)
            {
                UIRoot.ListView.OnMoveToLeft();    
            }
            else
            {
                UIRoot.ListView.OnMoveToRight();
            }
                
        }
        public void TestV(float v) 
        {
            if (openWinStack.Count <= 0)
            {
                return;
            }
            if (UIRoot.ListView == null)
            {
                return;
            }
            if (v < 0)
            {
                UIRoot.ListView.OnMoveToDown();                
            }
            else
            {
                UIRoot.ListView.OnMoveToUp();
            }
        }
    }
}