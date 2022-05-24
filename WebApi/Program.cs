using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Models;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Repositories;

namespace WebApi
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);


                    var communityRepository = services.GetRequiredService<ICommunityRepositoryAsync>();
                    var studentRepository = services.GetRequiredService<IStudentRepositoryAsync>();
                    var notificationRepository = services.GetRequiredService<INotificationRepositoryAsync>();
                    var eventRepository = services.GetRequiredService<IEventRepositoryAsync>();
                    var reportRepository = services.GetRequiredService<IReportRepositoryAsync>();

                    //var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();

                    await Infrastructure.Persistence.Seeds.DefaultCommunities.SeedAsync(communityRepository);
                    await Infrastructure.Persistence.Seeds.DefaultStudents.SeedAsync(studentRepository);
                    await Infrastructure.Persistence.Seeds.DefaultEvents.SeedAsync(eventRepository);
                    await Infrastructure.Persistence.Seeds.DefaultNotifications.SeedAsync(notificationRepository);;
                    await Infrastructure.Persistence.Seeds.DefaultReports.SeedAsync(reportRepository);

                    Log.Information("Finished Seeding Default Data");
                    Log.Information("Application Starting");
                    host.Run();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "An error occurred seeding the DB");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }

        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
