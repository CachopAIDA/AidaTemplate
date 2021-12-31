namespace AidaTemplate.Api.Options {
    public class CustomersApiOptions {
        public static string HttpClientId => "CustomersApiClient";
        public string BaseUrl { get; set; }
        public AuthServerOptions AuthServer { get; set; } = new AuthServerOptions();
    }

    public class AuthServerOptions {
        public string Url { get; set; }
        public string Scope { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public OAuth2PolicyOptions Policy { get; set; } = new OAuth2PolicyOptions();
    }

    public class OAuth2PolicyOptions {
        public bool RequireHttps { get; set; }
        public bool ValidateIssuerName { get; set; }
        public bool ValidateEndpoints { get; set; }
    }
}