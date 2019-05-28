using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretNet.Common.Providers.Storage
{
    public interface IStorage : IDisposable
    {
        void Create<T>(string key, T value);
        void Update<T>(string key, T value);
        void Delete(string key);
        T Read<T>(string key);

        ISerializer Serializer { get; set; }
        IProtector Protector { get; set; }
    }
}
