using Hangfire;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireDemo.Job
{ 
    public class DummyStaticJob
    {
        private readonly IDummySyncInfoService dummySyncInfoService;

        public DummyStaticJob(IDummySyncInfoService dummySyncInfoService)
        {
            this.dummySyncInfoService = dummySyncInfoService;
        }
        public void Run()
        {
            var infosTask = dummySyncInfoService
                .GetDummySyncInfos();
            Task.WaitAll(infosTask);
            var infos = infosTask.Result;

            foreach (var item in infos)
            {
                RecurringJob.AddOrUpdate<DummyJob>("DummyJob" + item.Id, x => x.RunSyncJob(item.Id), "*/3 * * * *");
            }
        }
    }
}
