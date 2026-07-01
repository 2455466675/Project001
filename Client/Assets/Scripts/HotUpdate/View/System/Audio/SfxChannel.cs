using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.View.Audio
{
    // 短音效通道：高并发、一次性播放，不循环、不淡变。内部用 AudioSource 池复用，
    // AudioClip 的加载/释放全部走 Game.Assets 引用计数，本通道只负责取源、播放、到时回收。
    internal class SfxChannel
    {
        private const int Prewarm = 8;

        private readonly AudioSourcePool pool;
        private float volumeScale = 1f;

        public SfxChannel(Transform root)
        {
            pool = new AudioSourcePool(root, Prewarm);
        }

        // 优先同步加载：命中 Game.Assets 缓存时可立即返回，避免高频调用引入一帧延迟。
        public void Play(string path, float volume)
        {
            AudioClip clip = Game.Assets.LoadAsset<AudioClip>(path);
            if (clip == null)
            {
                MDebug.Error($"音效加载失败 : {path}");
                return;
            }
            PlayClip(clip, volume);
        }

        public async UniTask PlayAsync(string path, float volume)
        {
            AudioClip clip = await Game.Assets.LoadAssetAsync<AudioClip>(path);
            if (clip == null)
            {
                MDebug.Error($"音效加载失败 : {path}");
                return;
            }
            PlayClip(clip, volume);
        }

        public void SetVolumeScale(float scale)
        {
            volumeScale = Mathf.Clamp01(scale);
        }

        private void PlayClip(AudioClip clip, float volume)
        {
            AudioSource source = pool.Get();
            source.clip = clip;
            source.loop = false;
            source.volume = Mathf.Clamp01(volume) * volumeScale;
            source.Play();
            RecycleAfterPlay(source, clip).Forget();
        }

        // 播放结束后归还 AudioSource 并释放 clip 的逻辑引用；归零的资源留待 UnloadUnusedAssets 统一回收，
        // 因此反复播放同一音效不会造成加载抖动。
        private async UniTaskVoid RecycleAfterPlay(AudioSource source, AudioClip clip)
        {
            int ms = Mathf.CeilToInt(clip.length * 1000f);
            await UniTask.Delay(ms);
            pool.Return(source);
            Game.Assets.ReleaseAsset(clip);
        }
    }
}
