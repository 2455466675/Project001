namespace Game.GSystem 
{
    public class BattleBehaviorComponent : UnitComponent, IAwakeComponent, IFixedUpdateComponent
    {
        private bool isRunning;
        private BattleAttributeComponent attributeComponent;

        public void Awake() 
        {
            isRunning = false;
            attributeComponent = GetComponent<BattleAttributeComponent>();
        }

        public void FixedUpdate(float dt)
        {
            if (!isRunning) 
            {
                return;
            }

            var sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
            var sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);
            var spr = attributeComponent.GetFinalValue(AttributeDefine.SP_RATE);
            if (sp2 >= sp1) 
            {
                sp2 = 0;
            }
            else
            {
                sp2 += spr;
                sp2 = GameMathf.Min(sp1, sp2);
            }
            attributeComponent.SetAttributeValue(AttributeDefine.SP_2, sp2);
        }

        public void StartUp()
        {
            isRunning = true;
        }

        public void ShutDown()
        {
            isRunning = false;
        }

        protected override void OnDestroyComponent()
        {
            isRunning = false;
            attributeComponent = null;
        }
    }
}