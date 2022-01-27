using Quartz;
using Quartz.Impl;

namespace Gradebook.Utils.PeriodicGradeSheet
{
    public class JobScheduler
    {
        public async static void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            IJobDetail job = JobBuilder.Create<GradeEmailJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithCronSchedule("0 0/5 * 1/1 * ? *")
                .WithIdentity("run infinitely every 5 minutes")
                .Build();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
