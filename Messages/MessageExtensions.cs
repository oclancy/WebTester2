using ProtoBuf;
using System.IO;

namespace Messages
{
    public static class MessageExtensions
    {
        public static byte[] Serialise(this IMessage message)
        {
            var stream = new MemoryStream();
            Serializer.SerializeWithLengthPrefix(stream, message, PrefixStyle.Base128);

            return stream.GetBuffer();
        }

        public static IMessage Deserialize(this byte[] payload)
        {
            var stream = new MemoryStream(payload);
            return Serializer.DeserializeWithLengthPrefix<IMessage>(stream, PrefixStyle.Base128);
        }
    }
}
