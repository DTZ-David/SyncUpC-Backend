using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class ForumService : IForumService
{
    private readonly IGenericRepository<Forum> _forumRepository;

    public ForumService(IGenericRepository<Forum> facultyRepository)
    {
        _forumRepository = facultyRepository;
    }

    public async Task<Forum> AddComment(string forumId, Comment comment)
    {
        var forum = await _forumRepository.GetById(forumId)
            ?? throw new BusinessException("El foro no existe", (int)MessageStatusCode.NotFound);

        forum.Comments.Add(comment);
        await _forumRepository.Update(forum);

        return forum;
    }

    public async Task<Forum> AddTopic(Forum forum)
    {
        await _forumRepository.Add(forum);
        return forum;
    }

    public async Task<List<Comment>> GetAllCommentForTopic(string forumId)
    {
        var forun = await _forumRepository.GetById(forumId);
        return forun.Comments;
    }

    public async Task<Forum> GetForum(string forumId)
    {
        var forun = await _forumRepository.GetById(forumId);
        return forun;
    }

    public async Task<List<Forum>> GetTopics(string eventId)
    {
        var forumTopics = await _forumRepository.FindAsync(f => f.EventId.Equals(eventId));
        return forumTopics.ToList();
    }
}
