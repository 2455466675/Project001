using GameFramework.Logic;
using GameFramework.Utility.GameDefine;
using MVC;

namespace GameFramework.View.UI
{
    [NavigationController(NavigationDefine.TestList1)]
    public class ListControllerSample1 : NavigationController<UISampleData>
    {
        protected override void RegisterData()
        {
            DataModelList<UISampleData> datas = new DataModelList<UISampleData>();
            for (int i = 0; i < 100; i++)
            {
                datas.Add(new UISampleData() { Id = i, Name = "name_fluid_" + i });
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
            Game.GetSystem<UISystem>().Navigate(NavigationDefine.TestList2);
        }
    }
}