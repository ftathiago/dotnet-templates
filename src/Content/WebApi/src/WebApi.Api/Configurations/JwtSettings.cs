using System;

namespace WebApi.Api.Configurations
{
    public class JwtSettings
    {
        public JwtSettings() =>
            ExpirationSeconds = TimeSpan.FromMinutes(30).TotalSeconds;

        public string Secret { get; set; }

        public double ExpirationSeconds { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
