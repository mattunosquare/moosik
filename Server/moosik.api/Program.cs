using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Validators;
using moosik.dal.Contexts;
using moosik.services.Dtos;
using moosik.services.Interfaces;
using moosik.services.Mapping;
using moosik.services.Services;
using Thread = moosik.dal.Contexts.Thread;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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

builder.Services.AddTransient<IThreadService, ThreadService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(cfg =>
{
    // cfg.AddProfile<EntityDtoMappingProfile>();
    //cfg.AddProfile<ViewModelDtoMappingProfile>();
     cfg.CreateMap<Post, PostDto>();
     cfg.CreateMap<PostResource, PostResourceDto>();
     cfg.CreateMap<ResourceType, ResourceTypeDto>();
     cfg.CreateMap<Thread, ThreadDto>();
     cfg.CreateMap<ICollection<Thread>, ICollection<ThreadDto>>();
    
     cfg.CreateMap<ThreadType, ThreadTypeDto>();
     cfg.CreateMap<ICollection<ThreadType>, ICollection<ThreadTypeDto>>();
     cfg.CreateMap<List<ThreadTypeDto>, List<ThreadTypeViewModel>>();
    
     cfg.CreateMap<User, UserDto>();
     cfg.CreateMap<ICollection<ThreadDto>, ICollection<ThreadViewModel>>();
});


builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddScoped(_ => new MoosikContext(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
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

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(x =>
{
    x.MapHealthChecks("/api/health");
    x.MapControllers();
});
app.Run();
