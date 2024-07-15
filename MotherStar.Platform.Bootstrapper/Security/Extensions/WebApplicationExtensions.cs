using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using MotherStar.Platform.Data.Security;
using MotherStar.Platform.Data;
using MotherStar.Platform.Domain.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MotherStar.Platform.Bootstrapper.Security.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task RunSecurityMigrations(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var securityDbContext = scope.ServiceProvider.GetService<SecurityDbContext>();
            securityDbContext.Database.Migrate();
            await SecuritySeedData.InitializeAsync(scope.ServiceProvider);
        }

        public static void UseSecurityApp(this WebApplication app)
        {
            // Create routes for the identity endpoints
            app.MapIdentityApi<AppUser>();


            // Provide an end point to clear the cookie for logout
            //
            // For more information on the logout endpoint and antiforgery, see:
            // https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
            app.MapPost("/logout", async (SignInManager<AppUser> signInManager, [FromBody] object empty) =>
            {
                if (empty is not null)
                {
                    await signInManager.SignOutAsync();

                    return Results.Ok();
                }

                return Results.Unauthorized();
            }).RequireAuthorization();

            app.MapGet("/roles", (ClaimsPrincipal user) =>
            {
                if (user.Identity is not null && user.Identity.IsAuthenticated)
                {
                    var identity = (ClaimsIdentity)user.Identity;
                    var roles = identity.FindAll(identity.RoleClaimType)
                        .Select(c =>
                            new
                            {
                                c.Issuer,
                                c.OriginalIssuer,
                                c.Type,
                                c.Value,
                                c.ValueType
                            });

                    return TypedResults.Json(roles);
                }

                return Results.Unauthorized();
            }).RequireAuthorization();

            /*app.MapPost("/data-processing-1", ([FromBody] FormDto model) =>
                Results.Text($"{model.Message.Length} characters"))
                    .RequireAuthorization();

            app.MapPost("/data-processing-2", ([FromBody] FormDto model) =>
                Results.Text($"{model.Message.Length} characters"))
                    .RequireAuthorization(policy => policy.RequireRole("Manager"));*/

        }
    }
}
