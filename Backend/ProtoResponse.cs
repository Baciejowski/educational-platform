using Google.Protobuf;

namespace Backend
{
    public static class ProtoResponse
    {
        public static Microsoft.AspNetCore.Mvc.IActionResult FromMsg(IMessage a)
        {
            return new Microsoft.AspNetCore.Mvc.FileContentResult(a.ToByteArray(), "application/x-protobuf");
        }
    }
}
