using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignUpProject.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SignUpProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SignUpProjectContext") ?? throw new InvalidOperationException("Connection string 'SignUpProjectContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.Scope = "openid profile email";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRole", policy =>
        policy.Requirements.Add(new RolesAuthorizationRequirement(new[] { "Admin" })));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffRole", policy =>
        policy.Requirements.Add(new RolesAuthorizationRequirement(new[] { "Staff" })));
});

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
