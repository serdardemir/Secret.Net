using SecretNet.Common.Types;
using System;

namespace SecretNet.Common.Events
{
    public class CrudEventArgs<T> : EventArgs
    {
        private T data;
        private CrudEvent crudEvent;
        private CrudEventTiming crudEventTiming;

        public CrudEventArgs(CrudEvent crudEvent, CrudEventTiming crudEventTiming, T data)
        {
            this.crudEvent = crudEvent;
            this.crudEventTiming = crudEventTiming;
            this.data = data;
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        public CrudEvent Event
        {
            get { return crudEvent; }
        }

        public CrudEventTiming EventTiming
        {
            get { return crudEventTiming; }
        }
    }
}