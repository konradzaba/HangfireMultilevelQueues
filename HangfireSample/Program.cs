using Hangfire;
using Hangfire.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HangfireSampleShared;

namespace HangfireSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitHangfireServersWithDashboard();
            
            Console.WriteLine("Hangfire Server 1 and Server 2 started...");

            // Enqueue test jobs
            Settings.ScheduleJobs();

            Console.WriteLine("jobs defined.");
            Console.Read();
        }

        /// <summary>
        /// Inits two Hangfire servers with Hangfire dashboard enabled under default address localhost:5000/hangfire
        /// </summary>
        private static void InitHangfireServersWithDashboard()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { ApplicationName = "HangfireSample", EnvironmentName = "Development" });
            builder.Host.ConfigureServices(
                (context, services) =>
                {
                    services.AddControllers();

                    // Configure Hangfire to use In-memory storage for sample
                    services.AddHangfire((provider, configuration) =>
                    {
                        configuration.UseInMemoryStorage();
                    });

                    #region Start two Hangfire server instances with different queues
                    services.AddHangfireServer(action =>
                    {
                        action.ServerName = Settings.Server1Name;
                        action.Queues = Settings.Server1Queues;
                        action.WorkerCount = Settings.Server1WorkerCount;
                    });

                    services.AddHangfireServer(action =>
                    {
                        action.ServerName = Settings.Server2Name;
                        action.Queues = Settings.Server2Queues;
                        action.WorkerCount = Settings.Server2WorkerCount;
                    });
                    #endregion
                });

            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter()]
            });

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.RunAsync();
        }
    }
}
