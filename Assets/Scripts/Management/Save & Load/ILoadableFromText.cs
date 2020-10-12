using System.IO;

namespace Miner.Management
{
    public interface ILoadable
    {
        void LoadFromStream(StreamReader stream);
    }
}