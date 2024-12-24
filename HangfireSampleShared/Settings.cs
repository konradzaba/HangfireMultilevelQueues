using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireSampleShared
{
    public static class Settings
    {
        public const string LowQueueServer1 = "low-queue-server-1";
        public const string LowQueueServer2 = "low-queue-server-2";
        public const string HighQueueServer1 = "high-queue-server-1";
        public const string HighQueueServer2 = "high-queue-server-2";

        public const string Server1Name = "Consumer1";
        public const string Server2Name = "Consumer2";

        public const int Server1WorkerCount = 1;
        public const int Server2WorkerCount = 1;

        public static string[] Server1Queues = { HighQueueServer1, LowQueueServer1 };
        public static string[] Server2Queues = { HighQueueServer2, LowQueueServer2 };

        public static void ScheduleJobs()
        {
            var jobTimeSeconds = 1;

            BackgroundJob.Enqueue(HighQueueServer1, () => Job.Sleep(jobTimeSeconds, HighQueueServer1));
            BackgroundJob.Enqueue(HighQueueServer2, () => Job.Sleep(jobTimeSeconds, HighQueueServer2));

            for (int i = 0; i < 10; i++)
            {

                BackgroundJob.Enqueue(LowQueueServer1, () => Job.Sleep(jobTimeSeconds, LowQueueServer1));
                BackgroundJob.Enqueue(HighQueueServer1, () => Job.Sleep(jobTimeSeconds, HighQueueServer1));
                BackgroundJob.Enqueue(LowQueueServer2, () => Job.Sleep(jobTimeSeconds, LowQueueServer2));
                BackgroundJob.Enqueue(HighQueueServer2, () => Job.Sleep(5, HighQueueServer2));
            }
        }
    }
}
