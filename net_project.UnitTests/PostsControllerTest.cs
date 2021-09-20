using System;
using Moq;
using Xunit;

using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Models;
using Microsoft.Extensions.Logging;
using net_project.Api.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using net_project.Api.Api.Dtos;
using FluentAssertions;
using System.Collections;
using System.Collections.Generic;

/* 
namespace net_project.UnitTests
{
    public class PostsControllerTest
    {
        
        // Mock => Moq
        private readonly Mock<PostsRepositoryInterface> repoStub = new();
        private readonly Mock<ILogger<PostsController>> loggerStub = new();
        
        private Random random_int = new();
        // Naming convention: UnitOfWork_StateUnderTest_ExpectedResultOrBehaviour

        // Check if GetPostAsync returns NotFound if no Post that fit criteria
        [Fact]
        public async Task GetPostAsync_WithUnexistingPost_ReturnsNotFound()
        {
            // Arrange
            repoStub.Setup(repo => repo.GetPostAsync(It.IsAny<Guid>()))
                                        .ReturnsAsync((Post)null);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetPostAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        // Check if GetPostAsync returns Post if found
        [Fact]
        public async Task GetPostsync_WithExistingPostReturnsExpectedPost()
        {
            // Arrange
            var expectedPost = CreateRandomPost();
            repoStub.Setup(repo => repo.GetPostAsync(It.IsAny<Guid>()))
                                        .ReturnsAsync(expectedPost);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetPostAsync(Guid.NewGuid());

            // Assert
            // Should => FluentAssertions
            result.Value.Should().BeEquivalentTo(expectedPost);
            
          
            // Assert.IsType<PostDto>(result.Value);
            // var dtoObj = (result as ActionResult<PostDto>).Value;
            // Assert.Equal(expectedPost.Id, dtoObj.Id);
            
        }

        // Check if GetPostsAsync returns all Posts in repository
        [Fact]
        public async Task GetPostsAsync_WithExistingPosts_ReturnsAllPosts()
        {
            // Arrange
            var expectedPosts = new[] { CreateRandomPost(), CreateRandomPost(), CreateRandomPost() };
            repoStub.Setup(repo => repo.GetPostsAsync())
                                    .ReturnsAsync(expectedPosts);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var actualPosts = await controller.GetPostsAsync();

            // Assert    
            actualPosts.Should().BeEquivalentTo(expectedPosts);
        }

        // Check if GetPostsAsync returns all Posts in repository
        [Fact]
        public async Task GetPostsAsync_WithMatchingPosts_ReturnsMatchingPosts()
        {
            // Arrange
            var expectedPosts = new[] 
            { 
                new Post() {Title = "Post 1", Content = "Content 1"},
                new Post() {Title = "Post 2", Content = "Content 2"},
                new Post() {Title = "Post 3", Content = "Content 3"},
                new Post() {Title = "Post 4", Content = "Content 4"},
            };

            var match = "Post 1";

            repoStub.Setup(repo => repo.GetPostsAsync())
                                    .ReturnsAsync(expectedPosts);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var actualPosts = await controller.GetPostsAsync(match);

            // Assert    
            actualPosts.Should().OnlyContain(
                Post => Post.Title == expectedPosts[0].Title
            );
        }

        // Check if PostPostAsync returns posted Post
        [Fact]
        public async Task PostPostAsync_WithPostToPost_ReturnsPostedPost()
        {
            // Arrange
            IList<Comment> comments = new List<Comment>();

            var PostToPost = new CreatePostDto(Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(),new User(), comments);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.PostPostAsync(PostToPost);

            // Assert
            var postedPost = (result.Result as CreatedAtActionResult).Value as PostDto;   

            result.Should().BeEquivalentTo(postedPost,
                options => options.ComparingByMembers<PostDto>().
                ExcludingMissingMembers());

            postedPost.Id.Should().NotBeEmpty();
            postedPost.CreatedDate.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(5));
        }

        // Check if PutPostAsync returns NoContent
        [Fact]
        public async Task PutPostAsync_WithPostToUpdate_ReturnsNoContent()
        {
            // Arrange
            var exisitingPost = CreateRandomPost();

            repoStub.Setup(repo => repo.GetPostAsync(It.IsAny<Guid>()))
                                    .ReturnsAsync(exisitingPost);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            var updatedPost = new UpdatePostDto(Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), exisitingPost.Price + 3);

            // Act
            var result = await controller.PutPostAsync(exisitingPost.Id, updatedPost);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        
        //Check if DeletePostAsync returns NoContent
        [Fact]
        public async Task DeletePostAsync_WithPostToDelete_ReturnsNoContent()
        {
            // Arrange
            var exisitingPost = CreateRandomPost();

            repoStub.Setup(repo => repo.GetPostAsync(It.IsAny<Guid>()))
                                    .ReturnsAsync(exisitingPost);

            var controller = new PostsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeletePostAsync(exisitingPost.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
        //public async Task DeletePostAsync_WithPostToDelete_ReturnsNoContent()  

        private Post CreateRandomPost()
        {
            return new()
            {
               Id = Guid.NewGuid(),
               User = new User(),
               Title = Guid.NewGuid().ToString(),
               Content = Guid.NewGuid().ToString(),
               CreatedDate = DateTimeOffset.UtcNow,
               Comments = {}
            };
        }
        private User CreateRandomUser()
        {
            return new()
            {
               Id = Guid.NewGuid(),
               FirstName = Guid.NewGuid().ToString(),
               LastName = Guid.NewGuid().ToString(),
               Biography = Guid.NewGuid().ToString(),
               ImageUrl = Guid.NewGuid().ToString(),
               Age = random_int.Next(10,70),
               CreatedDate = DateTimeOffset.UtcNow,
               Posts = {},
               Comments = {}
            };
        }
        private Comment CreateRandomComment()
        {
            return new()
            {
               Id = Guid.NewGuid(),
               Title = Guid.NewGuid().ToString(),
               Content = Guid.NewGuid().ToString(),
               CreatedDate = DateTimeOffset.UtcNow,
               User = new User(),
               Post = new Post()
            };
        }
    }
} 
*/
