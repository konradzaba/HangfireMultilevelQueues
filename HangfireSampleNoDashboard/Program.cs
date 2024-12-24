using Hangfire.InMemory;
using Hangfire;
using HangfireSampleShared;

namespace HangfireSampleNoDashboard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configure Hangfire to use In-memory
            GlobalConfiguration.Configuration.UseInMemoryStorage();

            // Start two Hangfire server instances with different queues
            var options1 = new BackgroundJobServerOptions
            {
                ServerName = Settings.Server1Name,
                Queues = Settings.Server1Queues,
                WorkerCount = Settings.Server1WorkerCount
            };

            var options2 = new BackgroundJobServerOptions
            {
                ServerName = Settings.Server2Name,
                Queues = Settings.Server2Queues,
                WorkerCount = Settings.Server2WorkerCount
            };

            using (var server1 = new BackgroundJobServer(options1))
            using (var server2 = new BackgroundJobServer(options2))
            {
                Console.WriteLine("Hangfire Server 1 and Server 2 started...");

                Settings.ScheduleJobs();

                Console.WriteLine("jobs defined.");
                Console.ReadLine();
            }
        }
    }
}
