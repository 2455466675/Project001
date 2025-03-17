using System.IO;

namespace Config
{
    /// <summary>
    /// 
    /// </summary>
	public interface IBinarySerialize
    {
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}

