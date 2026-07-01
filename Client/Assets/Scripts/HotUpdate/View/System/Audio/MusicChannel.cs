using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.View.Audio
{
    // BGM 单通道：逻辑上同一时刻只有一首曲子，但切歌时需要旧曲淡出、新曲淡入并存一小段，
    // 因此用两条淡变轨交叉，避免硬切换产生的爆音。
    internal class MusicChannel
    {
        private readonly FadeTrack[] tracks;
        private int activeIndex;
        private float volumeScale = 1f;

        public MusicChannel(Transform root)
        {
            tracks = new FadeTrack[2];
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i] = new FadeTrack(root, $"MusicTrack_{i}");
            }
        }

        public async UniTask PlayAsync(string path, float fadeTime)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            // 已在播放同一首，避免重复加载与无意义的重启。
            FadeTrack active = tracks[activeIndex];
            if (active.IsBusy && active.Path == path)
            {
                return;
            }

            AudioClip clip = await Game.Assets.LoadAssetAsync<AudioClip>(path);
            if (clip == null)
            {
                MDebug.Error($"BGM 加载失败 : {path}");
                return;
            }

            // 取另一条轨承载新曲。若它仍是上一次未结束的淡出轨，立即收尾释放，
            // 保证任意时刻至多一条轨处于淡出状态。
            int nextIndex = 1 - activeIndex;
            FadeTrack next = tracks[nextIndex];
            next.Release();

            active.FadeOut(fadeTime);
            next.SetVolumeScale(volumeScale);
            next.PlayFadeIn(path, clip, loop: true, fade: fadeTime);
            activeIndex = nextIndex;
        }

        public void Stop(float fadeTime)
        {
            tracks[activeIndex].FadeOut(fadeTime);
        }

        public void SetVolumeScale(float scale)
        {
            volumeScale = Mathf.Clamp01(scale);
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i].SetVolumeScale(volumeScale);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i].Update(deltaTime);
            }
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i].Release();
            }
        }
    }
}
