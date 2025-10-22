using System.Collections.Generic;
using System.Text;

namespace GameFramework.Gameplay
{
    public class InventoryItem
    {
        public string guid;
        public int id;
        public int count;
    }

    [Gameplay(-100)]
    public class InventorySystem : IGameplaySystem
    {
        public int id;
        private Dictionary<string, DataModel> inventory;

        public void OnInit()
        {
            inventory = new Dictionary<string, DataModel>();            
        }

        public void OnExit()
        {

        }

        public void Test() 
        {
            for (int i = 0; i < 100; i++)
            {
                DataModel item = new DataModel();
                string uid = SnowflakeGenerator.NextId().ToString();
                item.SetValue("uid", uid);
                item.SetValue("id", i);
                item.SetValue("count", UnityEngine.Random.Range(0, 100));
                inventory.Add(uid, item);
            }
        }

        public void OnSaveGame(ISaveWriter writer)
        {
            writer.Write("inventory", inventory);
        }

        public void OnLoadGame(ISaveReader reader)
        {
            inventory = reader.ReadDicWithStringKey("inventory");
            id = reader.ReadInt("id");
            //MDebug.Log("load", ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in inventory)
            {
                sb.Append(item.Value.ToString() + ";");
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}