using UnityEngine;
using System.Collections.Generic;
using GameFramework.Utility.GameDefine;
using System;

namespace GameFramework.Core 
{
    [CreateAssetMenu(menuName = "MyMenu/Create GameTransitionConfig")]
    public class GameTransitionConfig : ScriptableObject
    {
        public enum TransitionType_Config //TransitionType在Core程序集的定义。直接引用Utility的TransitionType的话[Serializable]会序列化失败
        {
            LoadGame,
            SwitchScene,
            EnterBattleScene,
            ExitBattleScene,
        }

        [Serializable]
        public class Config
        {
            public TransitionType_Config transitionType;
            /// <summary>
            /// 淡入时间(ms)
            /// </summary>
            public int fadeInTime;
            /// <summary>
            /// 最小过度时间(ms)
            /// </summary>
            public int transitionTime;
            /// <summary>
            /// 淡出时间(ms)
            /// </summary>
            public int fadeOutTime;
        }

        [SerializeField]
        private List<Config> configList;

        public int GetFadeInTime(TransitionType transitionType)
        {
            if (configList == null)
            {
                return 0;
            }

            var cfg = configList.Find(c => (TransitionType)c.transitionType == transitionType);
            if (cfg == null)
            {
                return 0;
            }

            return cfg.fadeInTime;
        }

        public int GetFadeOutTime(TransitionType transitionType)
        {
            if (configList == null)
            {
                return 0;
            }

            var cfg = configList.Find(c => (TransitionType)c.transitionType == transitionType);
            if (cfg == null)
            {
                return 0;
            }

            return cfg.fadeOutTime;
        }

        public int GetTransitionTime(TransitionType transitionType)
        {
            if (configList == null)
            {
                return 0;
            }

            var cfg = configList.Find(c => (TransitionType)c.transitionType == transitionType);
            if (cfg == null)
            {
                return 0;
            }

            return cfg.transitionTime;
        }
    }
}
