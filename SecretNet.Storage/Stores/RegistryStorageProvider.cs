using System;

namespace SecretNet.Storage.Stores
{
    public class RegistryStorageProvider : StorageProvider
    { 
        protected override void InternalCreate(string key, object data)
        {
            throw new NotImplementedException();
        }

        protected override void InternalDelete(string key)
        {
            throw new NotImplementedException();
        }

        protected override object InternalGet(string key)
        {
            throw new NotImplementedException();
        }

        protected override void InternalUpdate(string key, object data)
        {
            throw new NotImplementedException();
        }
    }
}