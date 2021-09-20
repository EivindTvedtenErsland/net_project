using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using net_project.Api.Api.Models;

namespace net_project.Api.Api.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    public record CreateItemDto([Required] string Name, string Description, [Range(1, 1000)] decimal Price);
    public record UpdateItemDto([Required] string Name, string Description, [Range(1, 1000)] decimal Price);
    public record PostDto(Guid Id, string Title, string Content, User user, IList<Comment> Comments, DateTimeOffset CreatedDate);
    public record CreatePostDto(string Title, string Content, User user, IList<Comment> Comments);
    public record UpdatePostDto(string Title, string Content, User user, IList<Comment> Comments);
    
}