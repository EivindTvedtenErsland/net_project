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

namespace net_project.UnitTests
{
    public class ItemsControllerTest
    {
        // Mock => Moq
        private readonly Mock<ItemsRepositoryInterface> repoStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        
        private Random random_int = new();
        
        // Naming convention: UnitOfWork_StateUnderTest_ExpectedResultOrBehaviour

        // Check if GetItemAsync returns NotFound if no item that fit criteria
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                                        .ReturnsAsync((Item)null);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        // Check if GetItemAsync returns item if found
        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = CreateRandomItem();

            repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                                        .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            // Should => FluentAssertions
            result.Value.Should().BeEquivalentTo(expectedItem);
            
            /*
            Assert.IsType<ItemDto>(result.Value);
            var dtoObj = (result as ActionResult<ItemDto>).Value;
            Assert.Equal(expectedItem.Id, dtoObj.Id);
            */
        }

        // Check if GetItemsAsync returns all items in repository
        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsAllItems()
        {
            // Arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repoStub.Setup(repo => repo.GetItemsAsync())
                                    .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var actualItems = await controller.GetItemsAsync();

            // Assert    
            actualItems.Should().BeEquivalentTo(expectedItems);
        }

        // Check if GetItemsAsync returns all items in repository
        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            // Arrange
            var expectedItems = new[] 
            { 
                new Item() {Name = "Holy Sword of Fish"},
                new Item() {Name = "Unholy Fragmented Caviar of Doom"},
                new Item() {Name = "Exalted Log of Justice"},
                new Item() {Name = "Dancing Fire Fish"}
            };

            var match = "Caviar";

            repoStub.Setup(repo => repo.GetItemsAsync())
                                    .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var actualItems = await controller.GetItemsAsync(match);

            // Assert    
            actualItems.Should().OnlyContain(
                                item => item.Name == expectedItems[1].Name);
        }

        // Check if PostItemAsync returns posted item
        [Fact]
        public async Task PostItemAsync_WithItemToPost_ReturnsPostedItem()
        {
            // Arrange
            var itemToPost = new CreateItemDto(Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), random_int.Next(100));

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.PostItemAsync(itemToPost);

            // Assert
            var postedItem = (result.Result as CreatedAtActionResult).Value as ItemDto;    
            result.Should().BeEquivalentTo(postedItem,
                options => options.ComparingByMembers<ItemDto>().
                ExcludingMissingMembers());

            postedItem.Id.Should().NotBeEmpty();
            postedItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(5));
        }

        // Check if PutItemAsync returns NoContent
        [Fact]
        public async Task PutItemAsync_WithItemToUpdate_ReturnsNoContent()
        {
            // Arrange
            var exisitingItem = CreateRandomItem();

            repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                                    .ReturnsAsync(exisitingItem);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            var updatedItem = new UpdateItemDto(Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), exisitingItem.Price + 3);

            // Act
            var result = await controller.PutItemAsync(exisitingItem.Id, updatedItem);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        
        //Check if DeleteItemAsync returns NoContent
        [Fact]
        public async Task DeleteItemAsync_WithItemToDelete_ReturnsNoContent()
        {
            // Arrange
            var exisitingItem = CreateRandomItem();

            repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                                    .ReturnsAsync(exisitingItem);

            var controller = new ItemsController(repoStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteItemAsync(exisitingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = random_int.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
