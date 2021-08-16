using ModelDTO;
using Newtonsoft.Json;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MattermostMessage : MessageService
    {
        private string _Url { get; set; }
        public MattermostMessage(string Url) : base()
        {
            this._Url = Url;
        }
        public override async Task SendMessage(MessageObj messageObj)
        {
            string msg = messageObj.GetType().GetProperties()
                .Select(prop => prop.GetValue(messageObj, null).ToString())
                .Aggregate((a, b) => $"{a} : {b}");

            string json = JsonConvert.SerializeObject(new { text = msg });
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _Url);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.SendAsync(requestMessage).Result.Content.ReadAsStringAsync();
            }
        }
    }
}
