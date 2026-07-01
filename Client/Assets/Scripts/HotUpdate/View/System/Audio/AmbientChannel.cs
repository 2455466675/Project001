using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework.View.Audio
{
    // 环境音通道：与 BGM 的根本区别是可多路同时叠加（风、雨、人群），且每路独立淡入淡出。
    // 因此用一组固定的淡变轨，以具名 key 管理，Play/Stop 均以 key 为单位。
    internal class AmbientChannel
    {
        private const int TrackCount = 4;

        private readonly FadeTrack[] tracks;
        private readonly Dictionary<string, int> keyToIndex;
        private float volumeScale = 1f;

        public AmbientChannel(Transform root)
        {
            tracks = new FadeTrack[TrackCount];
            keyToIndex = new Dictionary<string, int>();
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i] = new FadeTrack(root, $"AmbientTrack_{i}");
            }
        }

        public async UniTask PlayAsync(string key, string path, float fadeTime)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(path))
            {
                return;
            }

            // 同一 key 已在播放同一音源：无需重复加载。
            if (keyToIndex.TryGetValue(key, out int existIndex) && tracks[existIndex].Path == path)
            {
                return;
            }

            AudioClip clip = await Game.Assets.LoadAssetAsync<AudioClip>(path);
            if (clip == null)
            {
                MDebug.Error($"环境音加载失败 : {key} => {path}");
                return;
            }

            // 同一 key 换音源：旧轨转入淡出释放，再由新音源占用一条空闲轨。
            if (keyToIndex.TryGetValue(key, out existIndex))
            {
                tracks[existIndex].FadeOut(fadeTime);
                keyToIndex.Remove(key);
            }

            int index = FindFreeTrack();
            if (index < 0)
            {
                // 轨位已满时不静默吞掉资源，需归还引用避免泄漏。
                MDebug.Error($"环境音轨已满（{TrackCount}），无法播放 : {key}");
                Game.Assets.ReleaseAsset(clip);
                return;
            }

            FadeTrack track = tracks[index];
            track.SetVolumeScale(volumeScale);
            track.PlayFadeIn(path, clip, loop: true, fade: fadeTime);
            keyToIndex[key] = index;
        }

        public void Stop(string key, float fadeTime)
        {
            if (keyToIndex.TryGetValue(key, out int index))
            {
                tracks[index].FadeOut(fadeTime);
                keyToIndex.Remove(key);
            }
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
            keyToIndex.Clear();
        }

        // 空闲轨：已释放完资源的轨。淡出中的轨仍持有资源，视为占用，不会被新音源抢占。
        private int FindFreeTrack()
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                if (!tracks[i].IsBusy)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
