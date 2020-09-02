using Microsoft.AspNetCore.WebUtilities;

namespace Apae.Models
{
    public class ErrorViewModel
    {
        public int? Status { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string StatusDescription => Status.HasValue ?
            ReasonPhrases.GetReasonPhrase(Status.Value) : string.Empty;
    }
}