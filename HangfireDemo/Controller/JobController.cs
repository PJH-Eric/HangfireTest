using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireDemo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IDummySyncInfoService dummySyncInfoService;
        public JobController(IDummySyncInfoService dummySyncInfoService)
        {
            this.dummySyncInfoService = dummySyncInfoService;
        }

    }
}
