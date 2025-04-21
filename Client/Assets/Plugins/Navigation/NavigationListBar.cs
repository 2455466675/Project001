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
        private float showDuration;
        private float showDurationer;

        [SerializeField]
        private int tickCount;
        private int tickCounter;

        [SerializeField]
        private float tickInterval;
        private float tickIntervaler;

        [SerializeField]
        private float fadeIn;
        [SerializeField]
        private float fadeOut;

        private float size;
        private float value;

        private bool isShow;

        private void Awake()
        {
            if (list != null) 
            {
                list.OnListChangedEvent += List_OnListChangedEvent; ;
            }

            StartCoroutine(HideFade(0f, 0f));
        }

        private void Update()
        {
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

        private void List_OnListChangedEvent(ListChangedEventArgs obj)
        {
            int minIndex = obj.MinIndex;
            int itemCount = obj.ItemCount;
            int totalCount = obj.TotalCount;

            if (totalCount <= 0 || totalCount <= itemCount)
            {
                Hide();
                this.value = 0;
            }
            else
            {
                float size = (1f * itemCount) / totalCount;
                float value = (1f * minIndex) / (totalCount - itemCount);

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

        private void Show() 
        {
            if (isShow)
            {
                return;
            }
            StartCoroutine(ShowFade(1f, fadeIn));
        }

        private IEnumerator ShowFade(float v, float time)
        {
            if (canvasGroup == null)
            {
                yield break;
            }

            isShow = true;

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

            StartCoroutine(HideFade(0f, fadeOut));
        }

        private IEnumerator HideFade(float v, float time) 
        {
            if (canvasGroup == null)
            {
                yield break;
            }

            isShow = false;

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