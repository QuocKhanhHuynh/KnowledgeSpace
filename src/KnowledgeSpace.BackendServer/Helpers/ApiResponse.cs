using Newtonsoft.Json;

namespace KnowledgeSpace.BackendServer.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaulMasageForStatusCode(statusCode);
        }

        private static string GetDefaulMasageForStatusCode(int statusCode)
        {
            switch(statusCode)
            {
                case 400:
                    return "Resource not found";

                case 404:
                    return "An unhandled error occurred";

                default:
                    return null;
            }
        }
    }
}
