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
            for (int i = 0; i < 7; i++)
            {
                DataModel model = new DataModel();
                model.SetValue(DataKey.Id, i + 5);
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
        }
    }
}
