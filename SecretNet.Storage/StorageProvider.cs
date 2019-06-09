using SecretNet.Common.Events;
using SecretNet.Common.Providers.Storage;
using SecretNet.Common.Types;
using System;
using System.Collections;
using System.ComponentModel;

namespace SecretNet.Storage
{
    public abstract class StorageProvider : IStorage
    {
        private readonly EventHandlerList eventHandlers;
        private ISerializer serializer;
        private IProtector protector;

        protected StorageProvider()
        {
            this.eventHandlers = new EventHandlerList();
        }

        #region Abstract Methods

        protected abstract void InternalCreate(string key, object data);

        protected abstract void InternalUpdate(string key, object data);

        protected abstract void InternalDelete(string key);

        protected abstract object InternalGet(string key);

        #endregion Abstract Methods

        #region Methods

        protected void RaiseCrudEvent(ref string key, CrudEvent crudEvent, CrudEventTiming crudEventTiming)
        {
            object value = null;

            RaiseCrudEvent(ref key, ref value, crudEvent, crudEventTiming);
        }

        protected void RaiseCrudEvent(ref string key, ref object value, CrudEvent crudEvent, CrudEventTiming crudEventTiming)
        {
            EventHandler<CrudEventArgs<DictionaryEntry>> handler =
                eventHandlers["OnCrudEvent"] as EventHandler<CrudEventArgs<DictionaryEntry>>;

            if (handler != null)
            {
                CrudEventArgs<DictionaryEntry> args =
                    new CrudEventArgs<DictionaryEntry>(crudEvent, crudEventTiming, new DictionaryEntry(key, value));

                handler(this, args);

                key = (string)args.Data.Key;
                value = args.Data.Value;
            }
        }

        #endregion Methods

        #region IStorage Members

        public void Create<T>(string key, T value)
        {
            object valueToCreate = value;

            RaiseCrudEvent(ref key, ref valueToCreate, CrudEvent.Create, CrudEventTiming.Before);

            if (serializer != null && valueToCreate != null)
            {
                byte[] buffer = serializer.Serialize(typeof(T), valueToCreate);

                if (protector != null)
                {
                    valueToCreate = protector.Encrypt(buffer);
                }
                else
                {
                    valueToCreate = buffer;
                }
            }

            InternalCreate(key, valueToCreate);

            RaiseCrudEvent(ref key, ref valueToCreate, CrudEvent.Create, CrudEventTiming.After);
        }

        public void Update<T>(string key, T value)
        {
            object valueToUpdate = value;

            RaiseCrudEvent(ref key, ref valueToUpdate, CrudEvent.Update, CrudEventTiming.Before);

            if (serializer != null && valueToUpdate != null)
            {
                byte[] buffer = serializer.Serialize(typeof(T), valueToUpdate);

                if (protector != null)
                {
                    valueToUpdate = protector.Encrypt(buffer);
                }
                else
                {
                    valueToUpdate = buffer;
                }
            }

            InternalUpdate(key, valueToUpdate);

            RaiseCrudEvent(ref key, ref valueToUpdate, CrudEvent.Update, CrudEventTiming.After);
        }

        public void Delete(string key)
        {
            RaiseCrudEvent(ref key, CrudEvent.Delete, CrudEventTiming.Before);

            InternalDelete(key);

            RaiseCrudEvent(ref key, CrudEvent.Delete, CrudEventTiming.After);
        }

        public T Read<T>(string key)
        {
            RaiseCrudEvent(ref key, CrudEvent.Read, CrudEventTiming.Before);

            object value = InternalGet(key);

            if (serializer != null && value != null)
            {
                if (protector != null)
                {
                    value = protector.Decrypt((byte[])value);
                }

                value = serializer.Deserialize(typeof(T), (byte[])value);
            }

            RaiseCrudEvent(ref key, ref value, CrudEvent.Read, CrudEventTiming.After);

            return (T)value;
        }

        public event EventHandler<CrudEventArgs<DictionaryEntry>> OnCrudEvent
        {
            add { eventHandlers.AddHandler("OnCrudEvent", value); }
            remove { eventHandlers.RemoveHandler("OnCrudEvent", value); }
        }

        public ISerializer Serializer
        {
            get { return serializer; }
            set { serializer = value; }
        }

        public IProtector Protector
        {
            get { return protector; }
            set { protector = value; }
        }

        #endregion IStorage Members

        #region IDisposable Members

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeNativeAndManagedResources)
        {
            if (!disposed)
            {
                if (disposeNativeAndManagedResources)
                {
                    DisposeManagedResources();
                }

                DisposeNativeResources();

                disposed = true;
            }
        }

        protected virtual void DisposeManagedResources()
        {
            eventHandlers.Dispose();
        }

        protected virtual void DisposeNativeResources()
        {
        }

        #endregion IDisposable Members
    }
}