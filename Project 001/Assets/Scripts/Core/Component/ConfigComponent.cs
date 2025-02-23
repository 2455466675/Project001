using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Game.Cfg;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigComponent : EC.Component, IInitializable
    {
        public GameLanguage Language { get; private set; }
        public Formula Formula { get; private set; }
        private GameCfgData data;

        public IEnumerator Init(GameInitCfg intCfg)
        {
            Formula = World.GetComponent<ResourceComponent>().LoadAsset<Formula>(intCfg.FormulaFilePath);

            string filePath = Path.Combine(Application.streamingAssetsPath, intCfg.GameCfgFile);
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    data = new GameCfgData();
                    data.Deserialize(br);
                }
            }
            Language = new GameLanguage();

            yield return null;
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
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }
            var container = data.CfgDatas[t] as CfgContainerBase<T>;
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
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }
            var container = data.CfgDatas[t] as CfgContainerBase<T>;
            return container.Find(func);
        }

        /// <summary>
        /// 获取某类配置的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindAll<T>() where T : class, ICfg
        {
            Type t = typeof(T);
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }

            var container = data.CfgDatas[t] as CfgContainerBase<T>;
            return container.FindAll();
        }

        /// <summary>
        /// 获取满足条件的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<T> FindAll<T>(Func<T, bool> func) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }

            var container = data.CfgDatas[t] as CfgContainerBase<T>;
            return container.FindAll(func);
        }

        /// <summary>
        /// 通过配置id获取文体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTextById(int id)
        {
            return Language.GetTextById(id);
        }

        /// <summary>
        /// 获取语言文本配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LanguageItem GetLanguageItem(int id)
        {
            return Language.GetLanguageItem(id);
        }

        /// <summary>
        /// 通过id获取Color
        /// </summary>
        /// <param name="colorId"></param>
        /// <returns></returns>
        public Color GetColorById(int colorId)
        {
            return Language.GetColorById(colorId);
        }

        /// <summary>
        /// 通过HtmlString获取颜色
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public Color GetColorByHtmlStr(string htmlStr)
        {
            return Language.GetColorByHtmlStr(htmlStr);
        }
    }
}
