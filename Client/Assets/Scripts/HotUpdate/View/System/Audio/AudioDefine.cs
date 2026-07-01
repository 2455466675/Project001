namespace GameFramework.View.Audio
{
    // 音量分组按"混音需求"划分而非按内容：玩家通常需要独立调节音乐/音效/语音，
    // Master 作为全局总控叠乘在各组之上，便于一键静音或切后台压低整体音量。
    public enum AudioGroup
    {
        Master = 0,
        Music,
        Sfx,
        Voice,
        Ambient,
    }
}
