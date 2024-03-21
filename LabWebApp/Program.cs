using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LabWebApp.Data.ApplicationDbContext>(options =>
{
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true;");
});


builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<LabWebApp.Data.ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = "05a7c7692ffc898e5a16";
        o.ClientSecret = "9554b0a19cb5aa943574afb75119bb846179e5d5";
        o.CallbackPath = "/signin-github";
        // Grants access to read a user's profile data.
        o.Scope.Add("read:user");
        // Optional: if you need an access token to call GitHub APIs
        o.Events.OnCreatingTicket += context =>
        {
            if (context.AccessToken is { })
            {
                context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));
            }
            return Task.CompletedTask;
        };
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
