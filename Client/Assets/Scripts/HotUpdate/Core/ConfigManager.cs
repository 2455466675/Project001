using System.Collections.Generic;
using System;
using System.IO;
using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Core 
{
    public class ConfigManager : IGameModule
    {
        public class TextItem 
        {
            public string Text {  get; private set; }
            public Color Color { get; private set; }

            public TextItem(string text, Color color) 
            {
                Text = text; 
                Color = color;
            }
        }

        private GameCfgData m_Data;

        private Dictionary<int, TextItem> m_TextItems;

        public GameModulePriority Priority => GameModulePriority.ConfigManager;

        public async UniTask Init() 
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "cfg.bytes");
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    m_Data = new GameCfgData();
                    m_Data.Deserialize(br);
                }
            }

            m_TextItems = new Dictionary<int, TextItem>();

            await UniTask.Yield();
        }

        /// <summary>
        /// 通过配置id查找一个配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!m_Data.CfgDatas.ContainsKey(t))
            {
                return null;
            }
            var container = m_Data.CfgDatas[t] as CfgContainerBase<T>;
            return container.Find(id);
        }

        /// <summary>
        /// 查找配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Find<T>(Func<T, bool> func) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!m_Data.CfgDatas.ContainsKey(t))
            {
                return null;
            }
            var container = m_Data.CfgDatas[t] as CfgContainerBase<T>;
            return container.Find(func);
        }

        /// <summary>
        /// 获取某类配置的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FindAll<T>() where T : class, ICfg
        {
            Type t = typeof(T);
            if (!m_Data.CfgDatas.ContainsKey(t))
            {
                return new T[0];
            }

            var container = m_Data.CfgDatas[t] as CfgContainerBase<T>;
            return container.FindAll();
        }

        /// <summary>
        /// 获取满足条件的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T[] FindAll<T>(Func<T, bool> func) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!m_Data.CfgDatas.ContainsKey(t))
            {
                return new T[0];
            }

            var container = m_Data.CfgDatas[t] as CfgContainerBase<T>;
            return container.FindAll(func);
        }

        /// <summary>
        /// 通过配置id获取文体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTextById(int id)
        {
            var item = GetTextItem(id);
            if (item == null) 
            {
                return string.Empty;
            }
            else
            {
                return item.Text;
            }
        }

        public TextItem GetTextItem(int id) 
        {
            if (m_TextItems.TryGetValue(id, out var item))
            {
                return item;
            }
            else 
            {
                var cfg = Find<LanguageCfg>(id);
                if (cfg == null)
                {
                    return null;
                }
                var color = GetColor(cfg.Color);
                item = new TextItem(cfg.Text, color);
                m_TextItems.Add(id, item);
                return item;
            }
        }

        public Color GetColor(int colorId) 
        {
            var cfg = Find<ColorCfg>(colorId);
            if (cfg == null)
            {
                return Utility.Color.DefaultColor;
            }
            var color = Utility.Color.GetColorByHtmlStr(cfg.Color);
            return color;
        }
    }
}