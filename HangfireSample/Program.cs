using Hangfire;
using Hangfire.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using HangfireSampleShared;

namespace HangfireSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Start two Hangfire server instances with different queues

            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(new WebApplicationOptions() { ApplicationName = "HangfireSample", EnvironmentName = "Development" });
            builder.Host.ConfigureServices(
                (context, services) =>
                {
                    services.AddControllers();
                    services.AddHangfire((provider, configuration) =>
                    {
                        configuration.UseInMemoryStorage();
                    });

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

                });
            var app = builder.Build();


            app.UseHangfireDashboard("/hangfire", new
                        DashboardOptions
            {
                Authorization = new[] { new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter() }
            });

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.RunAsync();

            //---
            // Configure Hangfire to use In-memory
            Console.WriteLine("Hangfire Server 1 and Server 2 started...");

            // Enqueue a test job
            Settings.ScheduleJobs();

            Console.WriteLine("jobs defined.");
            Console.ReadLine();
        }
    }
}
