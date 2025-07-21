using MongoDB.Bson;
using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Forum
{
    public class Comment : BaseEntity<string>
    {
        public Comment(string forumId, string authorId, string content)
        {
            Id = ObjectId.GenerateNewId().ToString(); // ✅ Compatible con MongoDB
            CreationDate = DateTime.UtcNow;
            ForumId = forumId;
            AuthorId = authorId;
            Content = content;
        }

        public string ForumId { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
    }
}
