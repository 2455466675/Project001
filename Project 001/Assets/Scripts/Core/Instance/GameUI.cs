using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using UnityEditor.PackageManager.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameUI : MonoBehaviour, ICore
    {

        public UIRoot UIRoot {get; private set;}
        public Camera UICamera => UIRoot != null ? UIRoot.UICamera : null;

        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(GameCore.GameInitCfg.UIRootPath);
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

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

        public void RemoveSelectUIEventListener(UnityAction<IGuidable> action)
        {
            UIRoot.RemoveSelectUIEventListener(action);
        }

        public void RemoveDeselectUIEventListener(UnityAction<IGuidable> action)
        {
            UIRoot.RemoveDeselectUIEventListener(action);
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

        public void SelectListView(IGuidableGroup guidableGroup)
        {
            UIRoot.SelectListView(guidableGroup);
        }

        public void OpenWin(int id)
        {
            UIRoot.OpenWin(id);
        }

        public void CloseWin(int id)
        {
            UIRoot.CloseWin(id);
        }

        public void ExitUI()
        {
            UIRoot.ClearUI();
        }

        public void UndoCommand()
        {
            UIRoot.UndoCommand();
        }

        public void TestH(float h)
        {
            if (UIRoot.GuidableGroup == null)
            {
                return;
            }
            if (h < 0)
            {
                UIRoot.GuidableGroup.OnMoveToLeft();    
            }
            else
            {
                UIRoot.GuidableGroup.OnMoveToRight();
            }
                
        }
        public void TestV(float v) 
        {
            if (UIRoot.GuidableGroup == null)
            {
                return;
            }
            if (v < 0)
            {
                UIRoot.GuidableGroup.OnMoveToDown();                
            }
            else
            {
                UIRoot.GuidableGroup.OnMoveToUp();
            }
        }
    }
}