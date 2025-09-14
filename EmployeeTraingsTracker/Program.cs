using EmployeeTraingsTracker.Components;
using EmployeeTraingsTracker.Components.Account;
using EmployeeTraingsTracker.Data;
using EmployeeTraingsTracker.Model;
using EmployeeTraingsTracker.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTraingsTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -----------------------------
            // Database & Identity setup
            // -----------------------------
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Full Identity setup
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddRazorPages();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            // -----------------------------
            // Add custom services
            // -----------------------------
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ITrainingService, TrainingService>();
            builder.Services.AddScoped<IEmployeeTrainingService, EmployeeTrainingService>();
            // Add this after your Identity setup
            builder.Services.AddScoped<IdentityRedirectManager>();
            // -----------------------------
            // Add Blazor services
            // -----------------------------
            builder.Services.AddRazorComponents()
                   .AddInteractiveServerComponents();
            builder.Services.AddCascadingAuthenticationState();

            // -----------------------------
            // Build app
            // -----------------------------
            var app = builder.Build();

            // -----------------------------
            // Seed Roles and Admin User
            // -----------------------------
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "Employee" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Seed default admin
                string adminEmail = "admin@local.com";
                string adminPass = "Admin@123";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(adminUser, adminPass);
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // -----------------------------
            // HTTP Pipeline
            // -----------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery(); // Add this line!
            app.MapRazorPages();

            app.MapBlazorHub();
            app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

            // Fallback for root page using Razor Pages (_Host.razor)


            // Identity / Account endpoints
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
