using System;
using System.Collections.Generic;

using net_project.Api.Api.Repositories;
using net_project.Api.Api.Models;
using System.Threading.Tasks;

namespace net_project.Api.Api.Interfaces
{
    public interface PostsRepositoryInterface
    {
        Task<Post> GetPostAsync(Guid guid);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task PostPostAsync(Post post);
        Task PutPostAsync(Post post);
        Task DeletePostAsync(Post post);
        Task<IEnumerable<Comment>> GetCommentsOfPostAsync(Guid guid);
        Task<Comment> GetCommentOfPostAsync(Guid guid, string title = "");
    }
}