using WebApi.Options.Base;

namespace WebApi.Options
{
    public class VnpayOptions : AppOptions
    {
        private const string SectionName = "Vnpay";

        /// <summary>
        ///     Base Url of the VNPAY endpoint.
        /// </summary>
        public string BaseUrl { get; set; }

        public string Version { get; set; }

        public string Command { get; set; }

        public string CurrencyCode { get; set; }

        public string Locale { get; set; }

        public string TmnCode { get; set; }

        public string HashSecret { get; set; }

        public string ReturnUrl { get; set; }

        public override void Bind(IConfiguration configuration)
        {
            configuration
                .GetRequiredSection(SectionName)
                .Bind(this);
        }
    }
}
