using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignUpProject.Data;
using SignUpProject.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SignUpProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SignUpProjectContext") ?? throw new InvalidOperationException("Connection string 'SignUpProjectContext' not found.")));

// Add services to the container, localize views and data annotations.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
    .AddDataAnnotationsLocalization();

//configure Auth0 authentication
builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.Scope = "openid profile email";
    });

//add rule for admin role access
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRole", policy =>
        policy.Requirements.Add(new RolesAuthorizationRequirement(new[] { "Admin" })));
});

//add rule for staff role access
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffRole", policy =>
        policy.Requirements.Add(new RolesAuthorizationRequirement(new[] { "Staff" })));
});

//add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//add supported cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-UK", "fi" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddScoped<ModelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var supportedCultures = new[] { "en-UK", "fi" };

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
