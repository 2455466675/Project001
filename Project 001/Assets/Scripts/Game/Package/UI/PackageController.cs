using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageController : MonoBehaviour
	{
        [SerializeField]
        private StaticNavigationGroup menuList;
        [SerializeField]
        private LoopNavigationGroup itemList;

        private void Awake()
        {
            menuList.Init();

            itemList.Init();

            itemList.OnIndexChangedEvent += ItemList_OnIndexChangedEvent;

            itemList.UpdateElementCount(43);
        }

        private void ItemList_OnIndexChangedEvent(IndexChangedEventArgs obj)
        {
            MLog.Log("ItemList_OnIndexChangedEvent", obj.MinIndex, obj.MaxIndex);
            NavigationItem[] items = obj.Items;
            foreach (var item in items)
            {
                item.SetData(obj.MinIndex + item.Index);
            }
        }

        public void OnClickMenu(NavigationItem item)
        {
            GameWorld.Instance.GetComponent<UIComponent>().Navigate(UIDefine.Group_ID.Package_Item_Group);
        }

        public void OnSelectedMenu(NavigationItem item)
        {
            MLog.Log("OnSelectedMenu");
            itemList.UpdateElementCount(Random.Range(0, 4));
        }

        public void OnClickItem(NavigationItem item)
        {
            MLog.Log("OnClickItem", item.GetData());
            itemList.UpdateElementCount(43 + Random.Range(-5, 5));
        }
        public void OnSelectedItem(NavigationItem item)
        {
            MLog.Log("OnSelectedItem");
        }
    }
}

