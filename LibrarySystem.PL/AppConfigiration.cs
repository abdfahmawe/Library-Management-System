using LibrarySystem.BLL.Services.Classes;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.BLL.Setting;
using LibrarySystem.DAL.Data.Seed;
using LibrarySystem.DAL.Repositories.Classes;
using LibrarySystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.PL
{
    internal static class AppConfigiration
    {
        internal static void AddConfig(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<ILibraryItemRepository, LibraryItemRepository>();
            services.AddScoped<IDataSeed, DataSeed>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ILibraryItemService, LibraryItemService>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMemberCatalogService, MemberCatalogService>();
            services.AddScoped<IBorrowTransactionRepository, BorrowTransactionRepository>();
            services.AddScoped<IBorrowingService, BorrowingService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IReportService, ReportService>();
            
            // Email Sender Configuration
            services.Configure<EmailSettings>(
         configuration.GetSection("EmailSettings"));

            services.AddTransient<IEmailSender, EmailSender>();

            // Token time Configuration => 30 Min
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(30);
            });
        }
    }
}
