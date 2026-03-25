using MVC;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    [NavigationController(NavigationDefine.TestList2)]
    public class ListControllerSample2 : NavigationController<UISampleData>
    {
        protected override void OnShow()
        {
            ObservableList<UISampleData> datas = new ObservableList<UISampleData>();
            for (int i = 0; i < 4; i++)
            {
                datas.Add(new UISampleData() { Id = i, Name = "name_fixed_" + i });
            }

            SetData(datas);
        }

        protected override void OnHide()
        {
            //TODO
        }

        protected override void BindItemView(NavigationItemView itemView, UISampleData dataModel, Binder binder)
        {
            TextWidget textWidget = itemView.GetWidget<TextWidget>();
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
            //MDebug.Log("SelectItemView : ", dataModel.Name);
        }

        protected override void SubmitItemView(NavigationItemView itemView, UISampleData dataModel)
        {
            dataModel.Name = dataModel.Name + "Submit";
            //TODO
            //MDebug.Log("SubmitItemView : ", dataModel.Name);
        }
    }
}