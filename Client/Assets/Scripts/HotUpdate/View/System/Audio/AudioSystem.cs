using Cysharp.Threading.Tasks;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.View
{
    using Audio;

    // 音频系统作为 View 层的一个 GameSystem，只做对外门面：把播放请求转发给 BGM / 环境音 / 短音效三个通道，
    // 并统管音量分组。AudioClip 的加载/卸载全部复用 Game.Assets 引用计数。实现 IUpdateable 是为了驱动淡变。
    [GameSystem]
    public class AudioSystem : IGameSystem, IInit, IUpdateable
    {
        private const int GroupCount = 5;

        private MusicChannel music;
        private AmbientChannel ambient;
        private SfxChannel sfx;
        private float[] volumes;

        public void Init()
        {
            // 宿主节点依赖 GameRoot 预制件，而本 Init 早于 GameRoot 实例化，
            // 因此这里只初始化不依赖场景对象的音量状态，通道延迟到 EnsureHost 惰性构建。
            volumes = new float[GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                volumes[i] = 1f;
            }
        }

        public void Update(float deltaTime)
        {
            if (music != null)
            {
                music.Update(deltaTime);
                ambient.Update(deltaTime);
            }
        }

        #region BGM

        public UniTask PlayMusicAsync(string assetPath, float fadeTime = 1f)
        {
            if (!EnsureHost())
            {
                return UniTask.CompletedTask;
            }
            return music.PlayAsync(assetPath, fadeTime);
        }

        public void StopMusic(float fadeTime = 1f)
        {
            if (music != null)
            {
                music.Stop(fadeTime);
            }
        }

        #endregion

        #region 环境音

        // 环境音以 key 区分多路（如 "wind"、"rain"），可同时叠加，各自独立淡入淡出。
        public UniTask PlayAmbientAsync(string key, string assetPath, float fadeTime = 1f)
        {
            if (!EnsureHost())
            {
                return UniTask.CompletedTask;
            }
            return ambient.PlayAsync(key, assetPath, fadeTime);
        }

        public void StopAmbient(string key, float fadeTime = 1f)
        {
            if (ambient != null)
            {
                ambient.Stop(key, fadeTime);
            }
        }

        #endregion

        #region SFX

        public void PlaySfx(string assetPath, float volume = 1f)
        {
            if (!EnsureHost())
            {
                return;
            }
            sfx.Play(assetPath, volume);
        }

        public UniTask PlaySfxAsync(string assetPath, float volume = 1f)
        {
            if (!EnsureHost())
            {
                return UniTask.CompletedTask;
            }
            return sfx.PlayAsync(assetPath, volume);
        }

        #endregion

        #region 音量

        public void SetVolume(AudioGroup group, float value)
        {
            volumes[(int)group] = Mathf.Clamp01(value);
            // Master 会叠乘到所有组，各组变化也各自影响对应通道，因此统一下发一次。
            if (music != null)
            {
                music.SetVolumeScale(GetScale(AudioGroup.Music));
                ambient.SetVolumeScale(GetScale(AudioGroup.Ambient));
                sfx.SetVolumeScale(GetScale(AudioGroup.Sfx));
            }
        }

        public float GetVolume(AudioGroup group)
        {
            return volumes[(int)group];
        }

        // 某一组的实际生效音量 = 该组音量 * 总控音量。
        private float GetScale(AudioGroup group)
        {
            return volumes[(int)group] * volumes[(int)AudioGroup.Master];
        }

        #endregion

        // 场景切换时可调用：停掉所有 BGM/环境音轨并释放引用，配合 Game.Assets.UnloadUnusedAssetsAsync 回收。
        public void StopAll()
        {
            if (music != null)
            {
                music.ReleaseAll();
                ambient.ReleaseAll();
            }
        }

        // 惰性绑定宿主：首次真正播放音频时才从 GameRoot 取 AudioRoot 节点并构建各通道。
        private bool EnsureHost()
        {
            if (music != null)
            {
                return true;
            }

            AudioNode node = GameRoot.GetNode<AudioNode>();
            if (node == null)
            {
                MDebug.Error("AudioRoot 尚未就绪：GameRoot 未加载或预制件缺少 AudioNode");
                return false;
            }

            Transform root = node.transform;
            music = new MusicChannel(root);
            ambient = new AmbientChannel(root);
            sfx = new SfxChannel(root);
            music.SetVolumeScale(GetScale(AudioGroup.Music));
            ambient.SetVolumeScale(GetScale(AudioGroup.Ambient));
            sfx.SetVolumeScale(GetScale(AudioGroup.Sfx));
            return true;
        }
    }
}
