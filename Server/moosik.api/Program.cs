using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using moosik.api.Authentication.Interfaces;
using moosik.api.Authentication.Services;
using moosik.api.Authorization.Interfaces;
using moosik.api.Authorization.Services;
using moosik.api.Exception;
using moosik.api.ViewModels.Validators.User;
using moosik.dal.Contexts;
using moosik.dal.Interfaces;
using moosik.services.Interfaces;
using moosik.services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GeneralExceptionFilter>();
});
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>();
    fv.DisableDataAnnotationsValidation = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Moosik API",
        Description = "An ASP.NET Core Web API for managing Moosik items",
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidAccessToken", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
    
    options.AddPolicy("ValidRefreshToken", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddTransient<IThreadService, ThreadService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthorizedUserProvider, AuthorizedUserProvider>();


builder.Services.AddAutoMapper(config => config.AllowNullCollections = true, typeof(Program).Assembly);


builder.Services.AddFluentValidationRulesToSwagger();

// builder.Services.AddScoped(_ => new MoosikContext(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
//         "Host=localhost;Database=Moosik;Username=user;Password=password"));

builder.Services.AddScoped<IMoosikDatabase, MoosikContext>(_ => new MoosikContext(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                                                  "Host=localhost;Database=Moosik;Username=user;Password=password"));

builder.Services.AddHealthChecks();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseRouting();
app.UseCors(
    o => o
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()
);

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

app.UseEndpoints(x =>
{
    x.MapHealthChecks("/api/health");
    x.MapControllers();
});
app.Run();

public partial class Program
{
    
}
