using Apae.Data;
using Newtonsoft.Json;

namespace Apae.Emails
{

    public class ResetPasswordTemplateData : UserTemplateData
    {
        public ResetPasswordTemplateData(User user) : base(user) { }

        [JsonProperty("reset_link")]
        public string ResetPasswordLink { get; set; }

    }

}
