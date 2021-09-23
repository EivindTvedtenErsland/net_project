using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Models;
using net_project.Api.Api.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace net_project.Api.Api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepositoryInterface _itemsRepositoryInterface;
        ILogger<ItemsController> _logger;
        public ItemsController (ItemsRepositoryInterface itemsRepositoryInterface, ILogger<ItemsController> logger) 
        {
            this._logger = logger;
            this._itemsRepositoryInterface = itemsRepositoryInterface;
        }
     
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string match = null)
        {
            var items = (await _itemsRepositoryInterface.GetItemsAsync())
                                                        .Select( item => item.ItemAsDto());

            if (!string.IsNullOrWhiteSpace(match))
            {
                items = items.Where(item => item.Name.Contains(match, 
                                            StringComparison.OrdinalIgnoreCase));
            } 

            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items}");

            return items;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) 
        {
            var item = await _itemsRepositoryInterface.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }
            
            return item.ItemAsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostItemAsync(CreateItemDto itemDto)
        {
            Item item = new() 
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };

            await _itemsRepositoryInterface.PostItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.ItemAsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _itemsRepositoryInterface.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;
            existingItem.Description = itemDto.Description;

           await _itemsRepositoryInterface.PutItemAsync(existingItem);

           return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var RemoveItem = await _itemsRepositoryInterface.GetItemAsync(id);

            
            if (RemoveItem is null)
            {
                return NotFound();
            }

            await _itemsRepositoryInterface.DeleteItemAsync(RemoveItem);

            return NoContent();
        }
    }
}