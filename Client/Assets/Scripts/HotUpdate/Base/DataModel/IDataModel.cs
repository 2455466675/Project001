namespace GameFramework 
{
    public interface IWriteOnlyDataModel
    {
        public void SetValue(string key, int value);
        public void SetValue(string key, float value);
        public void SetValue(string key, bool value);
        public void SetValue(string key, string value);
    }

    public interface IReadOnlyDataModel
    {
        public int GetIntValue(string key);
        public float GetFloatValue(string key);
        public bool GetBoolValue(string key);
        public string GetStringValue(string key);
    }

    public interface IDataModel : IWriteOnlyDataModel, IReadOnlyDataModel
    {

    }
}