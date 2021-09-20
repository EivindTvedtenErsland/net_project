using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_project.Api.Api.Interfaces;

/*
namespace net_project.Api.Api.Controllers
{
     [ApiController]
     [Route("Posts")]
     public class PostsController : ControllerBase
     {
          private readonly PostsRepositoryInterface _postsRepositoryInterface;
          private readonly ILogger<PostsController> _logger;
          public PostsController(ILogger<PostsController> logger, PostsRepositoryInterface postsRepositoryInterface)
          {
               _logger = logger;
               _postsRepositoryInterface = postsRepositoryInterface;
          }

          [HttpGet]
          public async Task<IEnumerable<PostDto>> GetPostsAsync(string match = null)
          {
               var random_int = new Random();
          
          }
     }
}
  */