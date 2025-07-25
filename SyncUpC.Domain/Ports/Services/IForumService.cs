using SyncUpC.Domain.Entities.Forum;

namespace SyncUpC.Domain.Ports.Services
{
    public interface IForumService
    {
        Task<Forum> AddTopic(Forum forum);
        Task<List<Forum>> GetTopics(string eventId);
        Task<Forum> GetForum(string forumId);
        Task<Forum> AddComment(string forumId, Comment comment);
        Task<List<Comment>> GetAllCommentForTopic(string forumId);
    }
}
