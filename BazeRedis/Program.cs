using Microsoft.OpenApi.Models;
using ReaderCult.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using ReaderCult.Models;

var builder = WebApplication.CreateBuilder(args);

var redisClient = ConnectionMultiplexer.Connect("redis-19246.c8.us-east-1-2.ec2.cloud.redislabs.com:19246,password=gofc");
builder.Services.AddSingleton<IConnectionMultiplexer>(redisClient);

builder.Services.AddScoped<IRedisServices, RedisServices>();
builder.Services.AddScoped<IChatService,ChatService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
(builder.Configuration.GetSection("AppSettings:Token").Value))
    };
}).AddCookie();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins(new string[]
                    {
                        "http://localhost:5500",
                        "https://localhost:5500",
                        "http://127.0.0.1:5500",
                        "https://127.0.0.1:5500",
                        "http://localhost:7119",
                        "https://localhost:7119",
                        "http://127.0.0.1:7119",
                        "https://127.0.0.1:7119",
                        "http://localhost:5112",
                        "https://localhost:5112",
                        "http://127.0.0.1:5112",
                        "https://127.0.0.1:5112",
                        "http://localhost:3000",
                        "https://localhost:3000",
                         "http://localhost:3001",
                        "https://localhost:3001",
                         "http://localhost:3002",
                        "https://localhost:3002",
                        "http://localhost:8001",
                        "https://localhost:8001",
                        "http://localhost:6379",
                        "https://localhost:6379",
                        "http://127.0.0.1:6379",
                        "https://127.0.0.1:6379"
                        

                    }).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    }));
builder.Services.AddSignalR(option => option.EnableDetailedErrors=true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/ChatHub");
app.MapControllers();
using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var chatHub = serviceScope.ServiceProvider.GetService<IHubContext<ChatHub>>();

var channel = redisClient.GetSubscriber().Subscribe("groupChat");
channel.OnMessage(async (message) =>
{
    try
    {
        var mess = JsonSerializer.Deserialize<ChatRoomMessage>(message.Message.ToString());
        if (mess != null && chatHub != null)
        {
            await chatHub.Clients.All.SendAsync("message" ,mess);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e} ");
    }
});

app.Run();
