using System;

namespace SecretNet.Common.Providers.Storage
{
    public interface ISerializer : IDisposable
    {
        byte[] Serialize(Type objectType, object data);

        object Deserialize(Type objectType, byte[] data);
    }
}
