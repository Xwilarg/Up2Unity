using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace Assets.Up2Unity.Runtime
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]

    internal class Up2JamResponse<T>
    {
        public bool Success { set; get; }
        public ErrorData Error { set; get; }
        public T Data { set; get; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class ErrorData
    {
        public string Code { set; get; }
        public string Message { set; get; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class LoginData
    {
        public string DeviceCode { set; get; }
        public string VerificationUri { set; get; }
        public string UserCode { set; get; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class TokenData
    {
        public string Status { set; get; }
        public string Token { set; get; }
    }
}
