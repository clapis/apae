using Apae.Data;
using Newtonsoft.Json;

namespace Apae.Emails
{

    public abstract class UserTemplateData
    {
        public UserTemplateData(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            FullName = user.FullName;
        }

        [JsonProperty("user_fullname")]
        public string FullName { get; set; }

        [JsonProperty("user_firstname")]
        public string FirstName { get; set; }

        [JsonProperty("user_lastname")]
        public string LastName { get; set; }

    }

}
