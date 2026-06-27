using System.Collections.Generic;
using System;
using System.IO;
using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using GameFramework.Utility;

namespace GameFramework.Core
{
    public class GameConfigManager
    {

        internal GameConfigManager()
        {        
        }

        public class TextItem
        {
            public string Text { get; private set; }
            public TextItem(string text)
            {
                Text = text;
            }
        }

        private GameCfgData cfgData;
        private Dictionary<string, TextItem> textItems;

        public async UniTask Init()
        {
            TextAsset textAsset = await Game.Assets.LoadAssetAsync<TextAsset>("Assets/Bundles/Config/cfg");
            var data = CfgDecryptor.Decrypt(textAsset.bytes, "aabuen(k23Llg?");

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    cfgData = new GameCfgData();
                    cfgData.Deserialize(br);
                }
            }
            Game.Assets.ReleaseAsset(textAsset);
            textItems = new Dictionary<string, TextItem>();
        }

        /// <summary>
        /// 通过配置id查找一个配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class, ICfg
        {
            var container = cfgData.GetContainer<T>();
            if (container == null)
            {
                return null;
            }
            return container.Find(id);
        }
        public T Find<T>(string id) where T : class, ICfg
        {
            var container = cfgData.GetContainer<T>();
            if (container == null)
            {
                return null;
            }
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
            var container = cfgData.GetContainer<T>();
            if (container == null)
            {
                return null;
            }
            return container.Find(func);
        }

        /// <summary>
        /// 获取某类配置的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FindAll<T>() where T : class, ICfg
        {
            var container = cfgData.GetContainer<T>();
            if (container == null)
            {
                return new T[0];
            }
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
            var container = cfgData.GetContainer<T>();
            if (container == null)
            {
                return new T[0];
            }
            return container.FindAll(func);
        }

        /// <summary>
        /// 通过配置id获取文体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTextById(string id)
        {
            var item = GetTextItem(id);
            if (item == null)
            {
                return id;
            }
            else
            {
                return item.Text;
            }
        }

        public Color GetColor(int colorId)
        {
            var cfg = Find<ColorCfg>(colorId);
            if (cfg == null)
            {
                return Util.Color.DefaultColor;
            }
            var color = Util.Color.GetColorByHtmlStr(cfg.Color);
            return color;
        }

        private TextItem GetTextItem(string id)
        {
            if (textItems.TryGetValue(id, out var item))
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
                item = new TextItem(cfg.Text);
                textItems.Add(id, item);
                return item;
            }
        }
    }
}