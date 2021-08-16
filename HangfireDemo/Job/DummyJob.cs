using HangfireDemo.Domain;
using Microsoft.Extensions.Logging;
using ModelDTO;
using ModelDTO.Backstage;
using ModelDTO.SlotGame;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using Service.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace HangfireDemo.Job
{
    public class DummyJob
    {
        private readonly IDummySyncInfoService dummySyncInfoService;
        private readonly IMessageService messageService;
        private readonly ILogger<DummyJob> _logger;
        public DummyJob(IDummySyncInfoService dummySyncInfoService, ILogger<DummyJob> logger, IMessageService messageService)
        {
            this.dummySyncInfoService = dummySyncInfoService;
            _logger = logger;
            this.messageService = messageService;
        }
        //public async Task Run()
        //{
        //    await dummySyncInfoService.UpdateStatus(0);

        //    var infosTask = dummySyncInfoService
        //        .GetDummySyncInfos();
        //    Task.WaitAll(infosTask);
        //    var infos = infosTask.Result; 

        //    foreach (var item in infos)
        //    {
        //        await RunSyncJob(item);
        //    }
        //}

        public async Task RunSyncJob(int Id)
        {
            DummySyncInfo info = dummySyncInfoService.Get(Id).Result;
            if (info.Status == 1)
                return;
            await RunSyncJob(info);
        }

        private async Task RunSyncJob(DummySyncInfo info)
        {
            _logger.LogDebug("Id:" + info.Id.ToString() + "Running");
            await dummySyncInfoService.UpdateStatus(info.Id, 1);
            try
            {
                //var transactionOption = new TransactionOptions();
                //transactionOption.IsolationLevel = IsolationLevel.ReadUncommitted;
                // Act
                //using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOption,
                //    TransactionScopeAsyncFlowOption.Enabled))
                {
                    //取得host清單
                    _logger.LogDebug("Id:" + info.Id.ToString() + "GetHostList");
                    var hostListTask = dummySyncInfoService.GetHostList(info.HostList);
                    Task.WaitAll(hostListTask);
                    var hostApiKeyList = hostListTask
                                        .Result
                                        .ToList();

                    //取得指定db dummy資料
                    _logger.LogDebug("Id:" + info.Id.ToString() + "GetGameTableLogs");
                    IGameTableLogService gameTableLogService = new 
                        GameTableLogService(info.Fromdb, AppManager.AppSetting.DbConfig.User, AppManager.AppSetting.DbConfig.Password);

                    var tableLogTask = gameTableLogService.GetGameTableLogs(info.LastSyncId, hostApiKeyList.Select(x => x.HostExtId).ToList(), AppManager.AppSetting.limit);
                    Task.WaitAll(tableLogTask);
                    var dataLogs = tableLogTask.Result;

                    _logger.LogDebug("Id:" + info.Id.ToString() + "dataLogsHandle");
                    //根據設定每次處理並insert幾筆
                    for (int cnt = 0; cnt < dataLogs.Count(); cnt += AppManager.AppSetting.Interval.insert_count)
                    {
                        var takeData = dataLogs
                                       .Skip(cnt)
                                       .Take(AppManager.AppSetting.Interval.insert_count);
                        long thisId = takeData.Max(x => x.ID);

                        List<AccountingBase> accountings = new List<AccountingBase>();
                        List<AccountingBase> accountingRobots = new List<AccountingBase>();

                        //Parallel.ForEach(takeData, (item, loopState) =>
                        //處理每筆log
                        foreach (var item in takeData)
                        {
                            var membersLog = ParseMemberLog(item.Log);
                            var jObj = JObject.Parse(membersLog);

                            //將log拆分至accountings,accountingRobots
                            JToken memberJ = jObj.First;
                            while (memberJ != null)
                            {
                                AccountingBase accountingBase = new AccountingBase
                                {
                                    GameResult = item.vRoundID,
                                    SubGameId = item.vSubGameID,
                                    BeginTime = item.vTime,
                                    GameTime = item.vTime,
                                    GameId = info.GameId,
                                    GameData = item.Log,
                                    HostId = hostApiKeyList
                                           .FirstOrDefault(x => x.HostExtId == item.vHostApiKey)
                                           .HostId
                                };
                                var token = memberJ.First;

                                accountingBase.MemberId = Convert.ToString(token.SelectToken("nam"));
                                accountingBase.EndCent = Convert.ToInt64(token.SelectToken("gold"));
                                int win = (int)Convert.ToDouble(token.SelectToken("tot"));
                                if (win > 0)
                                {
                                    accountingBase.Bet = win;
                                    accountingBase.TotalWin = win * 2;
                                }
                                else
                                {
                                    accountingBase.Bet = -win;
                                    accountingBase.TotalWin = 0;
                                }

                                if (Convert.ToBoolean(token.SelectToken("bot")))
                                    accountingRobots.Add(accountingBase);
                                else
                                    accountings.Add(accountingBase);

                                memberJ = memberJ.Next;
                            }
                        }
                        //});
                        //寫入
                        _logger.LogDebug("Id:" + info.Id.ToString() + " Count:" + accountings.Count);
                        _logger.LogDebug("Id:" + info.Id.ToString() + " RobotsCount:" + accountingRobots.Count);
                        _logger.LogDebug("Id:" + info.Id.ToString() + " InsertSlotGameAccounting");
                        using (var scope = new TransactionScope())
                        {
                            IAccountingService accountingService = new
                                AccountingService(info.Todb, AppManager.AppSetting.DbConfig.User, AppManager.AppSetting.DbConfig.Password);

                            Task.WaitAll(accountingService.InsertAccountingRange(accountings),
                                accountingService.InsertAccountingRobotRange(accountingRobots));

                            scope.Complete();
                        }
                        await dummySyncInfoService.UpdateLastSyncData(info.Id, DateTime.UtcNow, thisId);

                        _logger.LogDebug("Id:" + info.Id.ToString() + " Sleep " + AppManager.AppSetting.Interval.break_time + "s");
                        Thread.Sleep(AppManager.AppSetting.Interval.break_time * 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                MessageObj messageObj = new MessageObj
                {
                    eventLevel = EventLevel.Error,
                    project = "NewDummySyncJob",
                    messgae = "Id:" + info.Id + " line:" + line + " Message: " + ex.Message
                };
                await messageService.SendMessage(messageObj);
                _logger.LogError(ex, info.Id.ToString());
                //await dummySyncInfoService.UpdateStatus(info.Id, 3);
            }
            finally
            {
                await dummySyncInfoService.UpdateLastSyncTime(info.Id, DateTime.UtcNow)
                    .ContinueWith(x => dummySyncInfoService.UpdateStatus(info.Id, 2));

                _logger.LogDebug("Id:" + info.Id.ToString() + "Completed");

                await Task.CompletedTask;
            }
        }
        private string ParseMemberLog(string log)
        {
            return JObject.Parse(log)
                                .DescendantsAndSelf()
                                .OfType<JProperty>()
                                .Single(x => x.Name.Equals("p"))
                                .Value.ToString();
        }
        private DateTime ConvertCulture(DateTime dateTime, string culture)
        {
            return Convert.ToDateTime(dateTime, new CultureInfo(culture));
        }
    }
}
