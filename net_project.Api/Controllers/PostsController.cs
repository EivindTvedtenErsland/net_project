using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_project.Api.Api.Dtos;
using net_project.Api.Api.Interfaces;

namespace net_project.Api.Api.Controllers
{
     [ApiController]
     [Route("posts")]
     public class PostsController : ControllerBase
     {
          private readonly PostsRepositoryInterface _postsRepositoryInterface;
          private readonly ILogger<PostsController> _logger;
          public PostsController(PostsRepositoryInterface postsRepositoryInterface, ILogger<PostsController> logger)
          {
               this._logger = logger;
               this._postsRepositoryInterface = postsRepositoryInterface;
          }

          [HttpGet]
          public async Task<IEnumerable<PostDto>> GetPostsAsync()
          {
               var posts = (await _postsRepositoryInterface.GetPostsAsync())
                                                            .Select(post => post.PostAsDto());

               _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {posts}");

               return posts;
          }
          [HttpGet("{id}")]
          public async Task<ActionResult<PostDto>> GetPostAsync(Guid id)
          {
               var post = (await _postsRepositoryInterface.GetPostAsync(id));

               if (post is null)
               {
                    return NotFound();
               }
               
               return post.PostAsDto();
          }

          [HttpPost]
          public async Task<ActionResult> PostPostAsync(CreatePostDto postDto)
          {
               
               return NoContent();
          }

          [HttpPut]
          public async Task<ActionResult> PutPostAsync(Guid guid, UpdatePostDto postDto)
          {
               return NoContent();
          }

          [HttpDelete]
          public async Task<ActionResult> DeletePostAsync (Guid guid)
          {
               return NoContent();
          }

          [HttpGet]
          public async Task<ActionResult> GetCommentOfPostAsync (Guid guid, string title)
          {
               return NoContent();
          }
          
          [HttpGet]
          public async Task<ActionResult> GetCommentsOfPostAsync (Guid guid)
          {
               return NoContent();
          }

     }
}
