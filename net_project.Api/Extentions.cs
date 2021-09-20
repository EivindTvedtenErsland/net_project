using net_project.Api.Api.Dtos;
using net_project.Api.Api.Models;

namespace net_project.Api.Api
{
    public static class Extentions
    {
        public static ItemDto ItemAsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
         
        }
        public static PostDto PostAsDto(this Post post)
        {
            return new PostDto(post.Id, post.Title, post.Content, post.User, post.Comments, post.CreatedDate);
         
        }

    }
}