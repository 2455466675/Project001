using MVC;
using GameFramework.Utility.GameDefine;
using System.Collections.Generic;

namespace GameFramework.View.UI
{
    [NavigationController(NavigationDefine.TestList1)]
    public class ListControllerSample1 : NavigationController<UISampleData>
    {
        protected override void OnShow()
        {
            ObservableList<UISampleData> datas = new ObservableList<UISampleData>();
            for (int i = 0; i < 100; i++)
            {
                datas.Add(new UISampleData());
            }

            SetData(datas);
        }

        protected override void OnHide()
        {
            //TODO
        }

        protected override void BindItemView(NavigationItemView itemView, UISampleData dataModel, Binder binder)
        {
            TextWidget textWidget = itemView.GetWidget<TextWidget>("widget_key");
            if (textWidget != null)
            {
                binder.Binding(textWidget, dataModel, d => d.Name, (v, d) => 
                {
                    v.SetText(d.Name);
                    //TODO
                });
            }
        }

        protected override void SelectItemView(NavigationItemView itemView, UISampleData dataModel)
        {
            //TODO
        }

        protected override void SubmitItemView(NavigationItemView itemView, UISampleData dataModel)
        {
            //TODO
        }
    }
}