using ModelDTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class MessageService : IMessageService
    {
        public MessageService()
        {

        }
        public abstract Task SendMessage(MessageObj obj);
    }
}
