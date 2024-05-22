using BookingProject.API.Middlewares;
using BookingProject.Application;
using BookingProject.Persistence;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices();
builder.Services.AddSingleton(sp => {
    string credentialsPath = builder.Configuration["GoogleCloud:ApiKey"];
    var credential = GoogleCredential.FromFile(credentialsPath);
    return StorageClient.Create(credential);
});
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In=ParameterLocation.Header,
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});
// Configure the HTTP request pipeline.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.MapIdentityApi<AppUser>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseMiddleware<TokenRefreshMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomExceptionhandler();
app.MapControllers();

app.Run();
