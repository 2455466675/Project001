using Game.UI;
using UnityEngine;

namespace Game.GSystem 
{
    public class BattleHudComponent : UnitComponent
    {
        private BattleHudViewController viewController;

        public void Init() 
        {
        }

        public void LoadHud() 
        {
            ActorComponent actor = GetComponent<ActorComponent>();
            if (actor == null || !actor.IsValid) 
            {
                return;
            }

            GameObject hudGo = Game.Resource.LoadAndInstantiate("Assets/Bundles/Battle/BattleHud", actor.GetHudNode());
            viewController = hudGo.GetComponent<BattleHudViewController>();
            viewController.UpdateHudPos(actor.GetBone("top"), actor.GetBone("bottom"));

            BattleAttributeComponent bac = GetComponent<BattleAttributeComponent>();
            bac.OnAttributeChangedEvent += OnAttributeChanged;

            UpdateAttributeView(AttributeDefine.SP_2);
            UpdateAttributeView(AttributeDefine.HP_2);

            UpdateNameView();
        }

        protected override void OnDestroyComponent()
        {
            BattleAttributeComponent bac = GetComponent<BattleAttributeComponent>();
            bac.OnAttributeChangedEvent -= OnAttributeChanged;

            GoHelper.Destroy(viewController.gameObject);
            viewController = null;
        }

        private void OnAttributeChanged(AttributeDefine arg)
        {
            UpdateAttributeView(arg);
        }

        private void UpdateAttributeView(AttributeDefine define) 
        {
            if (define == AttributeDefine.HP_1 || define == AttributeDefine.HP_2)
            {
                SliderView view = viewController.GetView<SliderView>("HP");
                if (view != null)
                {
                    BattleAttributeComponent bac = GetComponent<BattleAttributeComponent>();
                    int hp1 = bac.GetAttributeValue(AttributeDefine.HP_1);
                    int hp2 = bac.GetAttributeValue(AttributeDefine.HP_2);
                    view.SetValue(hp2, hp1);
                }
            }

            if (define == AttributeDefine.SP_1 || define == AttributeDefine.SP_2) 
            {
                SliderView view = viewController.GetView<SliderView>("SP");
                if (view != null)
                {
                    BattleAttributeComponent bac = GetComponent<BattleAttributeComponent>();
                    int sp1 = bac.GetAttributeValue(AttributeDefine.SP_1);
                    int sp2 = bac.GetAttributeValue(AttributeDefine.SP_2);
                    view.SetValue(sp2, sp1);
                }
            }
        }

        private void UpdateNameView() 
        {
            TextView view = viewController.GetView<TextView>("Name");
            if (view != null)
            {
                BattleCampComponent bcc = GetComponent<BattleCampComponent>();
                view.SetTextByStr(bcc.Name);
            }
        }
    }
}