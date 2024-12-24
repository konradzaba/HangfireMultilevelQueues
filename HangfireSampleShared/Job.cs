namespace HangfireSampleShared
{
    public static class Job
    {
        public static void Sleep(int seconds, string queueServer)
        {
            var sleepVariant = seconds > 1 ? "long sleep" : "sleep";
            Console.WriteLine($"{queueServer}| Start {sleepVariant} for {seconds}");
            Thread.Sleep(seconds * 1000);
            Console.WriteLine($"{queueServer}| End {sleepVariant} for {seconds}");
        }
    }
}
