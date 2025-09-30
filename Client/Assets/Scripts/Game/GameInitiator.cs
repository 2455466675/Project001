using UnityEngine;
using GameFramework;
using GameFramework.Core;
using GameFramework.Featrue;
using GameFramework.Gameplay;
using GameFramework.UI;

public class GameInitiator : MonoBehaviour
{
    private void Awake()
    {
        Core.Init();
        Feature.Init();
        Gameplay.Init();
        UI.Init();
    }

    private async void Start()
    {
        await Game.InitModules();

        Game.GetModule<InputController>().Switch(InputModuleType.Character);
        Game.GetModule<UIManager>().Navigate(NavigationDefine.TestList1);
    }
}
