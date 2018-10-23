using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSE545.Group8.Bank.WebAPI.Areas.HelpPage
{
    /// <summary>
    /// Indicates whether the sample is used for request or response
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SampleDirection
    {
        Request = 0,
        Response
    }
}