using SecretNet.Common.Providers.Storage;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecretNet.Storage.Serializers
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter formatter;

        public BinarySerializer()
        {
            formatter = new BinaryFormatter();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
        }

        public byte[] Serialize(Type objectType, object data)
        {
            byte[] result = null;

            using (MemoryStream stream = new MemoryStream(512))
            {
                formatter.Serialize(stream, data);

                result = stream.ToArray();
            }

            return result;
        }

        public object Deserialize(Type objectType, byte[] data)
        {
            object result = null;

            using (MemoryStream stream = new MemoryStream(data))
            {
                result = formatter.Deserialize(stream);
            }

            return result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}