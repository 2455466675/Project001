using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    [UIPanelController(PanelDefine.TestPanel2, NavigationDefine.TestList1, NavigationDefine.TestList2)]
    public class PanelControllerSample2 : PanelController
    {
        protected override string AssetPath => "Assets/Bundles/UI/Prefabs/Panel/TestPanel2";
        protected override void OnShow()
        {
            //UISampleData dataModel = new UISampleData();

            //this.Binder.Binding(dataModel, d => d.Name, (d) =>
            //{
            //    //TODO
            //});

            //this.Binder.Binding(dataModel, nameof(UISampleData.Name), (d) =>
            //{
            //    //TODO
            //});

            //this.Binder.Binding(dataModel, "Name", (d) =>
            //{
            //    //TODO
            //});

            //TextWidget textWidget = GetWidget<TextWidget>("widget_key");
            //if (textWidget != null )
            //{
            //    this.Binder.Binding(textWidget, dataModel, d => d.Id, (v, d) =>
            //    {
            //        //TODO
            //        v.SetText(d.Name);
            //        v.SetTextById(d.Id.ToString());
            //    });

            //    this.Binder.Binding(textWidget, dataModel, nameof(UISampleData.Id), (v, d) =>
            //    {
            //        //TODO
            //        v.SetText(d.Name);
            //        v.SetTextById(d.Id.ToString());
            //    });

            //    this.Binder.Binding(textWidget, dataModel, "Id", (v, d) =>
            //    {
            //        //TODO
            //        v.SetText(d.Name);
            //        v.SetTextById(d.Id.ToString());
            //    });
            //}
        }
    }
}