using System.Collections.Generic;
using GameFramework.Utility;
using UnityEngine;

namespace GameFramework.View.Audio
{
    // 短音效高频并发，若每次播放都 AddComponent/Destroy 会产生 GC 与开销尖峰，
    // 因此用对象池复用 AudioSource，播完归还而非销毁。
    internal class AudioSourcePool
    {
        private readonly Transform root;
        private readonly Stack<AudioSource> idle;
        private readonly List<AudioSource> all;

        public AudioSourcePool(Transform root, int prewarm)
        {
            this.root = root;
            idle = new Stack<AudioSource>();
            all = new List<AudioSource>();

            for (int i = 0; i < prewarm; i++)
            {
                idle.Push(Create());
            }
        }

        public AudioSource Get()
        {
            AudioSource source;
            if (idle.Count > 0)
            {
                source = idle.Pop();
            }
            else
            {
                source = Create();
            }
            source.gameObject.SetActive(true);
            return source;
        }

        public void Return(AudioSource source)
        {
            if (source == null)
            {
                return;
            }

            // 清空 clip 引用是关键：AudioSource 持有的 clip 会阻止 UnloadUnusedAssets 真正回收资源。
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            idle.Push(source);
        }

        private AudioSource Create()
        {
            GameObject go = GoHelper.CreateGameObject(root, $"SfxSource_{all.Count}");
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            go.SetActive(false);
            all.Add(source);
            return source;
        }
    }
}
