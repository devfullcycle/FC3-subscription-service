namespace Subscription.Infraestructure.ExternalService.Keycloak
{
    public class KeycloakConfig
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public string Realm { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
    }
}
