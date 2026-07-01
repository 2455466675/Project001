using GameFramework.Utility;
using UnityEngine;

namespace GameFramework.View.Audio
{
    // 单条淡变音轨：封装一个 AudioSource 的淡入/淡出与音量缩放。
    // 不负责资源加载（clip 由外部经 Game.Assets 加载后注入），但负责淡出结束后归还 clip 的逻辑引用，
    // 从而让 BGM（两轨交叉）与环境音（多轨叠加）复用同一套淡变与释放逻辑。
    internal class FadeTrack
    {
        private readonly AudioSource source;
        private AudioClip clip;
        private string path;
        private float factor;          // 当前淡变系数 [0,1]，最终音量 = factor * volumeScale
        private float volumeScale = 1f;

        private float fadeFrom;
        private float fadeTo;
        private float fadeTimer;
        private float fadeDuration;
        private bool fading;
        private bool releaseOnEnd;

        public FadeTrack(Transform root, string name)
        {
            GameObject go = GoHelper.CreateGameObject(root, name);
            source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }

        // 承载着资源即视为"忙"（播放中或淡出未结束），供调用方判断轨是否空闲可复用。
        public bool IsBusy => clip != null;
        public string Path => path;

        // 用外部已加载好的 clip 启动淡入。
        public void PlayFadeIn(string clipPath, AudioClip audioClip, bool loop, float fade)
        {
            if (clip != null)
            {
                Release();
            }
            path = clipPath;
            clip = audioClip;
            source.clip = audioClip;
            source.loop = loop;
            source.Play();
            StartFade(0f, 1f, fade, releaseOnEnd: false);
        }

        // 淡出并在结束后释放资源。
        public void FadeOut(float fade)
        {
            if (clip == null)
            {
                return;
            }
            StartFade(factor, 0f, fade, releaseOnEnd: true);
        }

        public void SetVolumeScale(float scale)
        {
            volumeScale = Mathf.Clamp01(scale);
            ApplyVolume();
        }

        public void Update(float deltaTime)
        {
            if (!fading)
            {
                return;
            }

            fadeTimer += deltaTime;
            float t = fadeDuration <= 0f ? 1f : Mathf.Clamp01(fadeTimer / fadeDuration);
            factor = Mathf.Lerp(fadeFrom, fadeTo, t);
            ApplyVolume();

            if (t >= 1f)
            {
                fading = false;
                if (releaseOnEnd && fadeTo <= 0f)
                {
                    Release();
                }
            }
        }

        public void Release()
        {
            fading = false;
            factor = 0f;
            if (clip != null)
            {
                source.Stop();
                source.clip = null;
                Game.Assets.ReleaseAsset(clip);
                clip = null;
                path = null;
            }
            ApplyVolume();
        }

        private void StartFade(float from, float to, float duration, bool releaseOnEnd)
        {
            fadeFrom = from;
            fadeTo = to;
            fadeTimer = 0f;
            fadeDuration = Mathf.Max(0f, duration);
            fading = true;
            this.releaseOnEnd = releaseOnEnd;
            factor = from;
            ApplyVolume();
        }

        private void ApplyVolume()
        {
            source.volume = factor * volumeScale;
        }
    }
}
