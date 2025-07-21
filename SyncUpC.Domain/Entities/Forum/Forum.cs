using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Forum
{
    public class Forum : BaseEntity<string>
    {
        public Forum(string eventId, string userId, string title, string content)
        {
            EventId = eventId;
            AuthorId = userId;
            Title = title;
            Content = content;
            Comments = new List<Comment>();
        }

        public string EventId { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; } = new();

    }
}
