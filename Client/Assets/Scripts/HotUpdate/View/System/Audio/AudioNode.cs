using GameFramework.Core;

namespace GameFramework.View.Audio
{
    // 挂在 GameRoot 预制件下的 AudioRoot 节点：作为所有音频 AudioSource 的常驻父节点，
    // 随 GameRoot 一起 DontDestroyOnLoad，从而跨场景保留 BGM 与音效池。
    public class AudioNode : GameNode
    {

    }
}
