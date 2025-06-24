using SyncUpC.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.User
{
    public class NotificationPreferences 
    {
        public NotificationPreferences(NotificationSetting eventReminders, NotificationSetting eventUpdates, NotificationSetting forumReplies, NotificationSetting forumMentions)
        {
            EventReminders = eventReminders;
            EventUpdates = eventUpdates;
            ForumReplies = forumReplies;
            ForumMentions = forumMentions;
        }

        public NotificationSetting EventReminders { get; set; }
        public NotificationSetting EventUpdates { get; set; }
        public NotificationSetting ForumReplies { get; set; }
        public NotificationSetting ForumMentions { get; set; }
    }
}
