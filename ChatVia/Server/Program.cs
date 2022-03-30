using AutoMapper;
using ChatVia.Application.Notifications;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Profiles;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MediatR;
using ChatVia.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(config 
    => config.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChatAppDB;Trusted_Connection=True;"));

builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password = new()
    {
        RequireDigit = false,
        RequiredLength = 1,
        RequireLowercase = false,
        RequireNonAlphanumeric = false,
        RequireUppercase = false,
    };

}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

builder.Services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
builder.Services.AddTransient(typeof(IEfRepository<>), typeof(EfRepository<>));

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
}).CreateMapper());

builder.Services.AddMediatR(
    typeof(Program).Assembly,
    typeof(MediatrDomainEventDispatcher).GetTypeInfo().Assembly);

builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});





var app = builder.Build();

// Database Seeding
using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
await AppDbContextSeed.Seed(context, userManager, logger);

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.MapFallbackToFile("index.html");

app.Run();
