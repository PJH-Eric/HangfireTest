using ModelDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IMessageService
    {
        Task SendMessage(MessageObj obj);
    }
}
