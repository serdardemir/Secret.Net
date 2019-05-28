using SecretNet.Common.Providers.Storage;
using System;

namespace SecretNet.Storage
{
    public abstract class StorageProvider : IStorage
    {
        public IProtector Protector
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ISerializer Serializer
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Create<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}