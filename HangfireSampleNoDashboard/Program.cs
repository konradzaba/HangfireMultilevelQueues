using Hangfire.InMemory;
using Hangfire;
using HangfireSampleShared;

namespace HangfireSampleNoDashboard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Hangfire configuration
            // Configure Hangfire to use In-memory
            GlobalConfiguration.Configuration.UseInMemoryStorage();

            // Configure two Hangfire server instances with different queues
            var server1Options = new BackgroundJobServerOptions
            {
                ServerName = Settings.Server1Name,
                Queues = Settings.Server1Queues,
                WorkerCount = Settings.Server1WorkerCount
            };

            var server2Options = new BackgroundJobServerOptions
            {
                ServerName = Settings.Server2Name,
                Queues = Settings.Server2Queues,
                WorkerCount = Settings.Server2WorkerCount
            };
            #endregion

            using var server1 = new BackgroundJobServer(server1Options);
            using var server2 = new BackgroundJobServer(server2Options);
            Console.WriteLine("Hangfire Server 1 and Server 2 started...");

            Settings.ScheduleJobs();

            Console.WriteLine("jobs defined.");
            Console.ReadLine();
        }
    }
}
