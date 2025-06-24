using SyncUpC.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.User
{
    public class NotificationSetting 
    {
        public NotificationSetting(bool push, bool email, bool whatsApp)
        {
            Push = push;
            Email = email;
            WhatsApp = whatsApp;
        }

        public bool Push { get; set; }
        public bool Email { get; set; }
        public bool WhatsApp { get; set; } // 👈 Nuevo canal

    }
}
