using BookingProject.API.Middlewares;
using BookingProject.Application;
using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Domain.Entities;
using BookingProject.Persistence;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

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
builder.Services.AddSwaggerGen();
// Configure the HTTP request pipeline.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomExceptionhandler();
app.MapControllers();

app.Run();
