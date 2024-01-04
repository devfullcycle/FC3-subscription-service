using Refit;

namespace Subscription.Infraestructure.ExternalService.Keycloak
{
    public interface IKeycloakApi
    {
        [Headers("Content-Type: application/x-www-form-urlencoded")]
        [Post("/realms/{realm}/protocol/openid-connect/token")]
        public Task<ApiResponse<string>> PostGetToken(
            [AliasAs("realm")] string realm,
            [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form,
            CancellationToken cancellationToken);

        [Get("/admin/realms/{realm}/clients?clientId={clientId}")]
        public Task<ApiResponse<List<KeycloakClientResponse>>> GetClientUUId(
           string realm,
           string clientId,
           [Header("Authorization")] string bearer,
           CancellationToken cancellationToken);

        [Get("/admin/realms/{realm}/clients/{clientUUId}/roles")]
        public Task<ApiResponse<List<KeycloakClientRole>>> GetRoles(
             [AliasAs("realm")] string realm,
             [AliasAs("clientUUId")] Guid clientUUId,
             [Header("Authorization")] string bearer,
             CancellationToken cancellationToken);

        [Get("/admin/realms/{realm}/users?email={userEmail}")]
        public Task<ApiResponse<List<KeycloakUserResponse>>> GetUser(
            [AliasAs("realm")] string realm,
            [AliasAs("userEmail")] string email,
            [Header("Authorization")] string bearer,
            CancellationToken cancellationToken);

        [Post("/admin/realms/{realm}/users/{userUUId}/role-mappings/clients/{roleId}")]
        public Task<IApiResponse> UpdateUserRole(
             [AliasAs("realm")] string realm,
             [AliasAs("userUUId")] Guid userUUId,
             [AliasAs("roleId")] Guid roleId,
             [Body] object roleData,
             [Header("Authorization")] string bearer,
             CancellationToken cancellationToken);

        [Delete("/admin/realms/{realm}/users/{userUUId}/role-mappings/clients/{roleId}")]
        public Task<IApiResponse> RemoveUserRole(
             [AliasAs("realm")] string realm,
             [AliasAs("userUUId")] Guid userUUId,
             [AliasAs("roleId")] Guid roleId,
             [Body] object roleData,
             [Header("Authorization")] string bearer,
             CancellationToken cancellationToken);
    }
}
