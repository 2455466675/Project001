using System;
using System.Collections.Generic;

namespace Game.GSystem 
{
    public enum AttributeDefine 
    {
        HP_1 = 1001,
        HP_2 = 1002,
        MP_1 = 1003,
        MP_2 = 1004,
        SP_1 = 1005,
        SP_2 = 1006,

        SP_RATE = 1101,
        ATK = 1102,
        DEF = 1103,
    }

    public class BattleAttributeComponent : UnitComponent
    {
        private const int erOffest = 10000;
        private const int epOffest = 20000;
        private const int edrOffest = 30000;
        private const int edpOffest = 40000;

        private readonly Dictionary<int, int> effects = new Dictionary<int, int>();       
        private readonly Dictionary<AttributeDefine, int> attributes = new Dictionary<AttributeDefine, int>();

        public event Action<AttributeDefine> OnAttributeChangedEvent;

        public void SetEffect(int effectId, int value)
        {
            if (effects.ContainsKey(effectId)) 
            {
                effects[effectId] = effects[effectId] + value;
            }
            else
            {
                effects.Add(effectId, value);
            }
        }

        public void SetAttributeValue(AttributeDefine define, int value) 
        {
            attributes[define] = value;
            OnAttributeChangedEvent?.Invoke(define);
        }

        public int GetAttributeValue(AttributeDefine define) 
        {
            if (attributes.ContainsKey(define)) 
            {
                return attributes[define];
            }
            else
            {
                return 0;
            }
        }

        public int GetFinalValue(AttributeDefine define) 
        {
            int baseValue = GetAttributeValue(define);

            int id = (int)define;
            int bValue = GetEffectValue(id);
            int erValue = GetEffectValue(id + erOffest);
            int epValue = GetEffectValue(id + epOffest);
            int edrValue = GetEffectValue(id + edrOffest);
            int edpValue = GetEffectValue(id + edpOffest);

            int value = GameMathf.Floor((baseValue + bValue) * (1 + erValue / 10000f) / (1 + edrValue / 10000f) + epValue - edpValue);
            return value;
        }

        private int GetEffectValue(int effectId)
        {
            if (effects.ContainsKey(effectId)) 
            {
                return effects[effectId];
            }
            else
            {
                return 0;
            }
        }
    }
}