using GameFramework.Logic;
using MVC;

namespace GameFramework.View.UI
{
    public partial class UISampleData : DataModel
    {
        [ObservableProperty] private int id;
        [ObservableProperty] private string name;
    }
}


