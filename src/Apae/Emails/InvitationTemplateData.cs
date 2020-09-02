using Apae.Data;
using Newtonsoft.Json;

namespace Apae.Emails
{

    public class InvitationTemplateData : UserTemplateData
    {
        public InvitationTemplateData(User user) : base(user) { }

        [JsonProperty("invited_by")]
        public string InvitedBy { get; set; }

        [JsonProperty("reset_link")]
        public string ResetPasswordLink { get; set; }

    }

}
