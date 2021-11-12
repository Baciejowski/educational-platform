using Google.Protobuf;
using Microsoft.AspNetCore.Http;

namespace Backend
{
    public class ProtoReader
    {
        public static T Convert<T>(HttpRequest request) where T : IMessage, new()
        {
            T message = new T();
            message.MergeFrom(request.BodyReader.AsStream());
            if (message.CalculateSize() == 0) return default(T);
            return message;
        }
    }
}
