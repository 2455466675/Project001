namespace Config
{
    public interface IConfig : IBinarySerialize
    {

    }

    public interface IConfig_IntKey : IConfig
    {
        int Id { get; }
    }

    public interface IConfig_StringKey : IConfig
    {
        string Id { get; }
    }
}