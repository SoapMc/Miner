using System.IO;

namespace Miner.Management
{
    public interface ISaveable
    {
        void WriteObjectToStream(StreamWriter stream);
    }
}