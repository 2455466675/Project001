namespace Config
{
    public interface ICfg : IBinarySerialize
    {
        int Id { get; }
    }
}