using System;
using UnityEngine;
using GameFramework.Core;
using System.Collections.Generic;
using LITJson;

namespace GameFramework.Logic
{
    public enum GameSaveType
    {
        Read,
        Write,
    }

    [GameSystem]
    public class GameSaveSummary : IGameSystem, IGameSaveSummary, IInit
    {
        public const int SAVE_SLOT_COUNT = 10;

        private static string SlotKey(int index) => $"SLOT_{index}";

        private const string ContentKey = "content";

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
                string json = "";          
                if (items.TryGetValue(SlotKey(i), out var slotData))
                {
                    json = slotData.ReadString(ContentKey);
                }                
                ((IGameSaveSummary)this).JsonToSummary(i, json);
            }
        }

        void IGameSaveSummary.Save(int index)
        {
            var targetSlot = slots[index];
            targetSlot.State = 1;
            targetSlot.Level = Utility.GameMath.Random(5, 27);
            targetSlot.Money = Utility.GameMath.Random(783, 2232);
            targetSlot.LastTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        void IGameSaveSummary.Delete(int index)
        {
            ((IGameSaveSummary)this).JsonToSummary(index, "");
        }

        void IGameSaveSummary.JsonToSummary(int index, string json)
        {
            if (index < 0 || index >= slots.Count)
            {
                return;
            }

            SaveSlotSummary summary;
            if (string.IsNullOrEmpty(json))
            {
                summary = new SaveSlotSummary();
            }
            else
            {
                summary = JsonMapper.ToObject<SaveSlotSummary>(json);
            }

            var slot = slots[index];
            slot.ApplySummary(summary);
        }

        string IGameSaveSummary.SummaryToJson(int index)
        {
            SaveSlotSummary summary;
            if (index < 0 || index >= slots.Count)
            {
                summary = new SaveSlotSummary();
            }
            else
            {
                summary = slots[index].ExportSummary();
            }
            return JsonMapper.ToJson(summary);
        }

        IGameSaveData IGameSaveSummary.Capture()
        {
            return BuildData();
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
                item.Write(ContentKey, JsonMapper.ToJson(slot.ExportSummary()));
                items.Add(key, item);
            }

            data.SetData(items);

            return data;
        }
    }
}
