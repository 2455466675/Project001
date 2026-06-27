using GameFramework.Core;
using GameFramework.Logic;
using MVC;
using System;
using UnityEngine;

namespace GameFramework.View.UI
{
    [NavigationController(Utility.GameDefine.NavigationDefine.GameSaveList)]
    public class GameSaveListController : NavigationController<GameSaveSlot>
    {
        protected override void RegisterData()
        {
            var slots = Game.GetSystem<GameSaveSummary>().GetSaveSlots();
            SetData(slots);
        }

        protected override void BindItemView(NavigationItemView itemView, GameSaveSlot dataModel, Binder binder)
        {
            var indexText = itemView.GetWidget<TextWidget>("Index");
            binder.Binding(indexText, dataModel, d => d.Index, (w, d) =>
            {
                w.SetText($"存档：{d.Index + 1}");
            });

            var state0 = itemView.GetWidget<AbsoluteWidget>("State0");
            binder.Binding(state0, dataModel, d => d.State, (w, d) =>
            {
                w.SetActive(d.State == 0);
            });

            var state1 = itemView.GetWidget<AbsoluteWidget>("State1");
            binder.Binding(state1, dataModel, d => d.State, (w, d) =>
            {
                w.SetActive(d.State == 1);
            });

            var levelText = itemView.GetWidget<TextWidget>("Level");
            binder.Binding(levelText, dataModel, d => d.Level, (w, d) =>
            {
                w.SetText($"等级：{d.Level}");
            });

            var moneyText = itemView.GetWidget<TextWidget>("Money");
            binder.Binding(moneyText, dataModel, d => d.Money, (w, d) =>
            {
                w.SetText($"金钱：{d.Money}");
            });

            var timeText = itemView.GetWidget<TextWidget>("Time");
            binder.Binding(timeText, dataModel, d => d.LastTime, (w, d) =>
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(d.LastTime).LocalDateTime;
                w.SetText($"存档时间：{dateTime:yyyy年MM月dd日 HH:mm:ss}");
            });
        }

        protected override void SelectItemView(NavigationItemView itemView, GameSaveSlot dataModel)
        {
            
        }

        protected override void SubmitItemView(NavigationItemView itemView, GameSaveSlot dataModel)
        {
            int index = dataModel.Index;
            bool isRead = GetContent<GameSaveType>() == GameSaveType.Read;
            if (isRead)
            {
                var hasData = Game.GetSystem<GameSaveSummary>().HasSaveData(index);
                if (hasData)
                {
                    MDebug.Log($"读档：{index}");
                    Game.GetSystem<GameStateSystem>().SetValue("save_index", index);
                    Game.GetSystem<GameStateSystem>().GamePhase = GamePhase.Play;
                }
                else
                {
                    MDebug.Log("此处没有存档", dataModel.Index);
                }
            }
            else
            {
                MDebug.Log($"存档：{index}");
                Game.GetSystem<GameSaveSystem>().SaveGame(index);
            }
        }
    }
}