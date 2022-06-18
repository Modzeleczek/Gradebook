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
                .WithCronSchedule("0 0/10 * 1/1 * ? *") // http://www.cronmaker.com
                .WithIdentity("run every 10th minute")
                .Build();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
