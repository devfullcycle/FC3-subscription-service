namespace Subscription.Infraestructure.ExternalService.Keycloak
{
    public class KeycloakUserResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
