using ChatApp.API.Channel;
using ChatApp.API.Repository;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterAuthentication(ref builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IDiscoverService, DiscoverService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ISenderService, SenderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MessageHub>("/chat-hub");

app.Run();

static void RegisterAuthentication(ref WebApplicationBuilder builder)
{
    var configuration= builder.Configuration;
    HubAuth(builder);
    ApiAuth(builder);

    builder.Services.AddAuthorization();
}

static void HubAuth(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication()
        .AddJwtBearer("HubAuth", cfg =>
        {
            cfg.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = async ctx =>
                {
                    try
                    {
                    }
                    catch (Exception exp)
                    {
                        throw;
                        //string errorMessage = exp.Message;
                    }
                },
                OnChallenge = async context =>
                {
                    try
                    {
                        context.HandleResponse();
                        var restultContext = context;
                        if (context.AuthenticateFailure != null)
                        {
                            context.Response.StatusCode = 401;
                            //// we can write our own custom response content here
                            await context.HttpContext.Response.WriteAsync("Token Validation Has Failed. Request Access Denied");
                        }


                    }
                    catch (Exception exp)
                    {
                        string errorMessage = exp.Message;
                    }
                },
                OnMessageReceived = async context =>
                {
                    try
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        var restultContext = context;
                        var token = context.Request.Query.FirstOrDefault(x => x.Key == "accessToken");
                        var username = context.Request.Query.FirstOrDefault(x => x.Key == "userName");
                        if (!string.IsNullOrEmpty(username.Value))
                        {
                            var appIdentity = new ClaimsIdentity("Custom");
                            //context.Token = token.Value;
                            appIdentity.AddClaim(new Claim(ClaimTypes.Name, username.Value));
                            context.Principal = new ClaimsPrincipal(appIdentity);
                            context.Success();
                        }
                        //return Task.CompletedTask;
                        ///////////

                    }
                    catch (Exception exp)
                    {
                        string errorMessage = exp.Message;
                    }
                },
                OnTokenValidated = async ctx =>
                {
                    try
                    {
                        var username = ctx.Request.Query.FirstOrDefault(x => x.Key == "userName");
                        if (!string.IsNullOrEmpty(username.Value))
                        {
                            var appIdentity = new ClaimsIdentity();
                            ctx.Principal.AddIdentity(appIdentity);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("[Failed in OnTokenValidated for RegisterJWTAuthenticationService] - ex.Message:[" + ex.Message + "] - " + ex.StackTrace);
                    }
                }
            };
        });
}
static void ApiAuth(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = async ctx =>
            {
                try
                {
                }
                catch (Exception exp)
                {
                    throw;
                    //string errorMessage = exp.Message;
                }
            },
            OnChallenge = async context =>
            {
                try
                {
                    context.HandleResponse();
                    var restultContext = context;
                    if (context.AuthenticateFailure != null)
                    {
                        context.Response.StatusCode = 401;
                        //// we can write our own custom response content here
                        await context.HttpContext.Response.WriteAsync("Token Validation Has Failed. Request Access Denied");
                    }


                }
                catch (Exception exp)
                {
                    string errorMessage = exp.Message;
                }
            },
            OnMessageReceived = async context =>
            {
                try
                {
                    var accessToken = context.Request.Headers["Authorization"];
                    // here you can validate the token and proceed
                    var appIdentity = new ClaimsIdentity("Custom");
                    //appIdentity.AddClaim();
                    context.Principal = new ClaimsPrincipal(appIdentity);
                    context.Success();

                }
                catch (Exception exp)
                {
                    string errorMessage = exp.Message;
                }
            },
            OnTokenValidated = async ctx =>
            {
                try
                {
                    var username = ctx.Request.Query.FirstOrDefault(x => x.Key == "userName");
                    if (!string.IsNullOrEmpty(username.Value))
                    {
                        var appIdentity = new ClaimsIdentity();
                        ctx.Principal.AddIdentity(appIdentity);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("[Failed in OnTokenValidated for RegisterJWTAuthenticationService] - ex.Message:[" + ex.Message + "] - " + ex.StackTrace);
                }
            }
        };
    });
}