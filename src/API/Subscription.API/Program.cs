using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Subscription.API.Routes;
using Subscription.Application.Abstractions;
using Subscription.Infraestructure.Database.Configuration;
using Subscription.Infraestructure.ExternalService.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPostgresDatabase(configuration);
builder.Services.AddApplication(configuration);
builder.Services.AddGateway(configuration);

builder.Services.AddKeycloakAuthentication(configuration, o =>
{
    o.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("manage", policy =>
    {
        policy
        .RequireRealmRoles("user")
        .RequireResourceRoles("admin");
    });
}).AddKeycloakAuthorization(configuration);


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.RegisterPlanEndpoints();
app.RegisterUserEndpoints();

app.UseAuthentication();
app.UseAuthorization();
app.Run();

public partial class Program { }