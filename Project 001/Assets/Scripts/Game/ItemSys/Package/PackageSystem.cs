using MVC;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageSystem : DataProxy
    {
        public PackageSystem(DataContainer container) : base(container)
        {
            DataCollection menuList = CreateCollection("MenuList");
            for (int i = 0; i < 5; i++)
            {
                DataContainer item = menuList.Append(true);
                item.SetBaseValue("id", i);
            }

            DataCollection itemList = CreateCollection("ItemList");
            for (int i = 0; i < 200; i++)
            {
                DataContainer item = itemList.Append(true);
                item.SetBaseValue("id", i + 1);
                item.SetBaseValue("name", $"{i + 1}--item");
                item.SetBaseValue("count", i * 2);
            }
        }

        public void Test()
        {
            DataCollection itemList = GetDataCollection("ItemList");

            for (int i = 0; i < 5; i++)
            {
                DataContainer item = itemList.Append(true);
                item.SetBaseValue("id", itemList.Count + i + 1);
                item.SetBaseValue("name", $"{itemList.Count + i + 1}--item");
                item.SetBaseValue("count", i * 2);
            }
            itemList.NotifyChanged();
        }

        public void Test1()
        {
            DataCollection itemList = GetDataCollection("ItemList");
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, itemList.Count);
                itemList.RemoveAt(index);
            }
            itemList.NotifyChanged();
        }
    }
}

