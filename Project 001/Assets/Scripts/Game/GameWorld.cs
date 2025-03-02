using EC;
using Game.Core;
using Game.UI;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameWorld : World
    {
        public static GameWorld Instance { get; private set; }

        public static void Init() 
        {
            Instance = new GameWorld();
        }

        public static IEnumerator Start(GameInitCfg intCfg) 
        {
            ResourceComponent rc = Instance.AddComponent<ResourceComponent>();
            yield return rc.Init(intCfg);

            ConfigComponent cc = Instance.AddComponent<ConfigComponent>();
            yield return cc.Init(intCfg);

            UIComponent uic = Instance.AddComponent<UIComponent>();
            yield return uic.Init(intCfg);

            InputComponent ic = Instance.AddComponent<InputComponent>();
            yield return ic.Init(intCfg);

            ic.SwitchInputMode(InputMode.UI);

            uic.Navigate(UIDefine.Group_ID.Test_Group_1);
            uic.GetNavigationGroup(UIDefine.Group_ID.Test_Group_1).SetUndoable(false);            

            yield return null;
        }

        public static void Step(float dt) 
        {
            Instance.Update(dt);
        }
    }
}
