
using Newtonsoft.Json;
using Subscription.Application.Adapters.AuthService;

namespace Subscription.Infraestructure.ExternalService.Keycloak
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IKeycloakApi _api;
        private readonly KeycloakConfig _config;

        public AuthorizationService(IKeycloakApi api, KeycloakConfig config)
        {
            _api = api;
            _config = config;
        }
        public async Task AddSubscriberRoleToUser(string email, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _api.PostGetToken(_config.Realm, new Dictionary<string, object>
                {
                    {"grant_type", "password" },
                    {"client_id",_config.ClientId },
                    {"username",_config.UserName },
                    {"password", _config.Password },
                    {"client_secret", _config.ClientSecret }
                }, cancellationToken);

                var token = JsonConvert.DeserializeObject<KeycloakTokenResponse>(data?.Content ?? "");

                if (!data.IsSuccessStatusCode)
                    throw new Exception(data.Error.Message);

                var client = await _api.GetClientUUId(
                                            _config.Realm,
                                            _config.ClientId,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
                if (!client.IsSuccessStatusCode)
                    throw new Exception(client.Error.Message);

                var roleData = await _api.GetRoles(
                                            _config.Realm,
                                            client?.Content[0]?.Id ?? Guid.Empty,
                                            "bearer " + token.AccessToken,
                                            cancellationToken );
                if (!roleData.IsSuccessStatusCode)
                    throw new Exception(roleData.Error.Message);

                var userData = await _api.GetUser(
                                            _config.Realm,
                                            email,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
                if (!userData.IsSuccessStatusCode)
                    throw new Exception(userData.Error.Message);

                var dataToSend = new[]
                {
                    new {id = roleData?.Content?.FirstOrDefault(item => item.Name == "subscriber")?.Id ?? Guid.Empty,
                        name = "subscriber"
                    }
                };

                var response = await _api.UpdateUserRole(
                                            _config.Realm,
                                            userData?.Content[0]?.Id ?? Guid.Empty,
                                            client?.Content[0]?.Id ?? Guid.Empty,
                                            dataToSend,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Keycloak service was down or with problem on add Subscriber");
            }
        }

        public async Task RemoveSubscriberRoleToUser(string email, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _api.PostGetToken(_config.Realm, new Dictionary<string, object>
                {
                    { "grant_type", "password" },
                    {"client_id",_config.ClientId },
                    {"username",_config.UserName },
                    {"password", _config.Password },
                    {"client_secret", _config.ClientSecret }
                }, cancellationToken);

                var token = JsonConvert.DeserializeObject<KeycloakTokenResponse>(data?.Content ?? "");

                if (!data.IsSuccessStatusCode)
                    throw new Exception(data.Error.Message);

                var client = await _api.GetClientUUId(
                                            _config.Realm,
                                            _config.ClientId,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
                if (!client.IsSuccessStatusCode)
                    throw new Exception(client.Error.Message);

                var roleData = await _api.GetRoles(
                                            _config.Realm,
                                            client?.Content[0]?.Id ?? Guid.Empty,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
                if (!roleData.IsSuccessStatusCode)
                    throw new Exception(roleData.Error.Message);

                var userData = await _api.GetUser(
                                            _config.Realm,
                                            email,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
                if (!userData.IsSuccessStatusCode)
                    throw new Exception(userData.Error.Message);

                var dataToSend = new[]
                {
                    new {id = roleData?.Content?.FirstOrDefault(item => item.Name == "subscriber")?.Id ?? Guid.Empty,
                        name = "subscriber"
                    }
                };

                var response = await _api.RemoveUserRole(
                                            _config.Realm,
                                            userData?.Content[0]?.Id ?? Guid.Empty,
                                            client?.Content[0]?.Id ?? Guid.Empty,
                                            dataToSend,
                                            "bearer " + token.AccessToken,
                                            cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Keycloak service was down or with problem on add Subscriber");
            }
        }
    }
}
