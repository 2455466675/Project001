using GameFramework.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI
{
    [NavigationController(NavigationDefine.BattleActionList2)]
    public class BattleActionList2Controller : NavigationController
    {
        protected override void OnShow()
        {
            List<DataModel> list = new List<DataModel>();
            int count = Utility.Math.Random(2, 8);
            for (int i = 0; i < count; i++)
            {
                DataModel model = new DataModel();
                model.SetValue(DataKey.Id, count * 10 + i);
                list.Add(model);
            }

            SetData(list);
        }

        protected override void RefreshItemView(NavigationItemView itemView, IReadOnlyDataModel dataModel)
        {
            TextWidget text = itemView.GetWidget<TextWidget>();
            if (text != null)
            {
                text.SetText(dataModel.GetStringValue(DataKey.Id));
            }
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int id = dataModel.GetIntValue(DataKey.Id);
            MDebug.Log("id = ", id);
            Game.GetSystem<BattleSystem>().FlowManager.MoveNext();
        }
    }
}
