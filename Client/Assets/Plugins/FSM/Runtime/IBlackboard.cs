namespace FSM
{
    public interface IBlackboard
    {
        void SetValue(string key, int value);
        void SetValue(string key, float value);
        void SetValue(string key, bool value);
        void SetValue(string key, string value);
        int GetIntValue(string key);
        float GetFloatValue(string key);
        bool GetBoolValue(string key);
        string GetStringValue(string key);
    }
}
