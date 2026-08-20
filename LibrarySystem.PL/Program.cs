using LibrarySystem.BLL.Services.Classes;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.BLL.Setting;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Data.Seed;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Classes;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace LibrarySystem.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // —»ÿ ﬁ”„ Jwt „‰ appsettings.json „⁄ JwtSettings
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(
                    JwtSettings.SectionName));

            // ﬁ—«¡… ≈⁄œ«œ«  JWT Õ Ï Ì” Œœ„Â« JwtBearer
            JwtSettings jwtSettings =
                builder.Configuration
                    .GetSection(JwtSettings.SectionName)
                    .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "JWT settings are missing.");

            //  ⁄—Ì› ÿ—Ìﬁ… «· Õﬁﬁ „‰ JWT «·ﬁ«œ„… „⁄ Requests
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtSettings.Issuer,

                            ValidateAudience = true,
                            ValidAudience = jwtSettings.Audience,

                            ValidateLifetime = true,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtSettings.Key)),

                            ClockSkew = TimeSpan.Zero
                        };
                });
            builder.Services.AddScoped<ILibraryItemRepository, LibraryItemRepository>();
            builder.Services.AddScoped<IDataSeed, DataSeed>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<ILibraryItemService, LibraryItemService>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMemberCatalogService, MemberCatalogService>();
            builder.Services.AddScoped<IBorrowTransactionRepository, BorrowTransactionRepository>();
            builder.Services.AddScoped<IBorrowingService, BorrowingService>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (IServiceScope scope =
                   app.Services.CreateScope())
            {
                IDataSeed dataSeed =
                    scope.ServiceProvider
                        .GetRequiredService<IDataSeed>();

                await dataSeed.SeedDataAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}