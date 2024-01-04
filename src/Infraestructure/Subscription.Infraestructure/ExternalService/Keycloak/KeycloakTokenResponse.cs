using Newtonsoft.Json;

namespace Subscription.Infraestructure.ExternalService.Keycloak
{
    public class KeycloakTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
