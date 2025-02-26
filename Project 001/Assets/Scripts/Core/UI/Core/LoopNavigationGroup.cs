using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class LoopNavigationGroup : NavigationGroup
    {
        public void Init(int count) 
        {
            Debug.Log("count:" + count);

            item.gameObject.SetActive(false);

            float vh = viewport.rect.size.y;
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 1);
            tf.anchorMax = new Vector2(0.5f, 1);
            tf.pivot = new Vector2(0.5f, 0.5f);

            if (!content.TryGetComponent<VerticalLayoutGroup>(out var layoutGroup))
            {
                MLog.Warn("LoopNavigationList初始化，content没有LayoutGroup");
                return;
            }

            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);

            float topPadding = layoutGroup.padding.top;
            float bottomPadding = layoutGroup.padding.bottom;
            float spacing = layoutGroup.spacing;

            float h = tf.rect.size.y;
            int c = Mathf.FloorToInt((vh - topPadding - bottomPadding) / (h + spacing / 2));    //计算个数

            for (int i = 0; i < c; i++)
            {
                GameObject lt = GoHelper.Instantiate(item.gameObject, content);

                lt.SetActive(true);
            }
        }
    }
}
