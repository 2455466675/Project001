using GameFramework.Core;
using System;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameSaveSummary : IGameSystem, IGameSaveSummary, IInit
    {
        public const int SAVE_SLOT_COUNT = 10;

        private static string SlotKey(int index) => $"SLOT_{index}";
        
        private DataModelList<GameSaveSlot> slots;

        void IInit.Init()
        {
            Game.GetSystem<GameSaveSystem>().RegisterSaveSummaryHandler(this);

            slots = new DataModelList<GameSaveSlot>(SAVE_SLOT_COUNT);
            for (int i = 0; i < SAVE_SLOT_COUNT; i++)
            {
                GameSaveSlot slot = new GameSaveSlot();
                slot.Index = i;
                slot.State = 0;
                slots.Add(slot);
            }

        }

        void IGameSaveSummary.Load(IGameSaveData data)
        {
            var items = data.GetData();

            for (int i = 0; i < SAVE_SLOT_COUNT; i++)
            {
                var key = SlotKey(i);
                var slot = slots[i];
                if (items.TryGetValue(key, out var slotData))
                {
                    slot.State = slotData.ReadInt("state");
                    slot.Level = slotData.ReadInt("level");
                    slot.Money = slotData.ReadInt("money");
                    slot.LastTime = slotData.ReadLong("last_time");
                }
                else
                {
                    slot.State = 0;
                    slot.Level = 0;
                    slot.Money = 0;
                }
            }

        }

        IGameSaveData IGameSaveSummary.Save(int index)
        {
            var targetSlot = slots[index];
            targetSlot.State = 1;
            targetSlot.Level = Utility.Util.Math.Random(5, 27);
            targetSlot.Money = Utility.Util.Math.Random(783, 2232);
            targetSlot.LastTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            return BuildData();
        }

        IGameSaveData IGameSaveSummary.Delete(int index)
        {
            var targetSlot = slots[index];
            targetSlot.State = 0;
            targetSlot.Level = 0;
            targetSlot.Money = 0;
            targetSlot.LastTime = 0;

            return BuildData();
        }

        private IGameSaveData BuildData()
        {
            GameSaveData data = new GameSaveData();
            Dictionary<string, IGameSaveItem> items = new Dictionary<string, IGameSaveItem>();
            for (int i = 0; i < SAVE_SLOT_COUNT; i++)
            {
                var key = SlotKey(i);
                var slot = slots[i];
                GameSaveItem item = new GameSaveItem();
                item.Write("index", slot.Index);
                item.Write("state", slot.State);
                item.Write("last_time", slot.LastTime);
                item.Write("level", slot.Level);
                item.Write("money", slot.Money);
                items.Add(key, item);
            }

            data.SetData(items);

            return data;
        }

        /// <summary>
        /// 是否有任意存档数据
        /// </summary>
        /// <returns></returns>
        public bool HasAnySaveData()
        {
            foreach (var item in slots)
            {
                if (item.State != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 存档栏是否有数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasSaveData(int index)
        {
            if (index < 0 || index >= slots.Count)
            {
                return false;
            }
            return slots[index] != null && slots[index].State != 0;
        }

        public DataModelList<GameSaveSlot> GetSaveSlots()
        {
            return slots;
        }
    }
}
