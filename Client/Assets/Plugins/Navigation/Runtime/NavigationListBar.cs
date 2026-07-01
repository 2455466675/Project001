using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
    public class NavigationListBar : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Scrollbar scrollbar;

        [SerializeField]
        private FluidNavigationList list;

        [SerializeField]
        private int tickCount;  //连续几次触发才显示滑动条
        private int tickCounter;

        [SerializeField]
        private float tickInterval; //连续检测时间
        private float tickIntervaler;

        [SerializeField]
        private float showDuration; //滑动条显示持续时间
        private float showDurationer;

        [SerializeField]
        private float fadeIn;
        [SerializeField]
        private float fadeOut;

        private float size;
        private float value;

        private bool isInFocus;
        private bool isShow;

        private void Awake()
        {
            if (list != null) 
            {
                list.OnItemBindData += List_OnListChangedEvent;
                list.OnListInFocus += List_OnListInFocusEvent;
                list.OnListOutFocus += List_OnListOutFocusEvent;
                list.OnListExit += List_OnListExitEvent;
            }

            StartCoroutine(HideFade(0f, 0f));
        }

        private void Update()
        {
            if (!isInFocus)
            {
                return;
            }

            if (tickCounter <= 0)
            {
                Hide();
                return;
            }

            if (tickCounter < tickCount)
            {
                if (tickIntervaler > 0f)
                {
                    tickIntervaler -= Time.deltaTime;
                }
                else
                {
                    tickCounter = 0;
                    tickIntervaler = 0f;
                }
            }
            else
            {
                if (showDurationer > 0)
                {
                    Show();
                    SetSize(size);
                    SetValue(value);
                    showDurationer -= Time.deltaTime;
                }
                else
                {
                    tickCounter = 0;
                    tickIntervaler = 0f;
                }
            }
        }

        private void List_OnListChangedEvent(NavigationItem item)
        {
            if (!isInFocus)
            {
                return;
            }

            int minIndex = list.MinIndex;
            int itemCount = list.ItemCount;
            int dataCount = list.DataCount;

            if (dataCount <= 0 || dataCount <= itemCount)
            {
                Hide();
                this.value = 0;
                tickCounter = 0;
                tickIntervaler = 0f;
            }
            else
            {
                float size = (1f * itemCount) / dataCount;
                float value = (1f * minIndex) / (dataCount - itemCount);
                if (this.value != value)
                {
                    this.size = size;
                    this.value = value;
                    tickCounter++;
                    tickIntervaler = tickInterval;
                    showDurationer = showDuration;
                }
            }
        }

        private void List_OnListOutFocusEvent()
        {
            isInFocus = false;
        }

        private void List_OnListInFocusEvent()
        {
            isInFocus = true;
        }

        private void List_OnListExitEvent()
        {
            isInFocus = false;
            Hide();
            this.value = 0f;
            tickCounter = 0;
            tickIntervaler = 0f;
            showDurationer = 0f;
        }

        private void Show() 
        {
            if (isShow)
            {
                return;
            }
            isShow = true;
            StartCoroutine(ShowFade(1f, fadeIn));
        }

        private IEnumerator ShowFade(float v, float time)
        {
            if (canvasGroup == null)
            {
                yield break;
            }

            float start = canvasGroup.alpha;

            float timer = 0f;
            while (timer < time)
            {
                canvasGroup.alpha = Mathf.Lerp(start, v, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = v;
        }

        private void Hide() 
        {
            if (!isShow) 
            {
                return;
            }
            isShow = false;
            StartCoroutine(HideFade(0f, fadeOut));
        }

        private IEnumerator HideFade(float v, float time) 
        {
            if (canvasGroup == null)
            {
                yield break;
            }

            float start = canvasGroup.alpha;

            float timer = 0f;
            while (timer < time) 
            {
                if (isShow) 
                {
                    yield break;
                }

                canvasGroup.alpha = Mathf.Lerp(start, v, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = v;
        }

        private void SetValue(float v) 
        {
            if (scrollbar != null) 
            {
                isShow = true;
                scrollbar.value = v;
            }
        }

        private void SetSize(float size) 
        {
            if (scrollbar != null) 
            {
                scrollbar.size = size;
            }
        }
    }
}