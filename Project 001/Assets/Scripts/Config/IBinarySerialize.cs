using System.IO;

namespace Game.Cfg
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

