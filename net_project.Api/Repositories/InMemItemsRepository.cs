using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Models;

namespace net_project.Api.Api.Repositories
{

    class InMemItemsRepository : ItemsRepositoryInterface
    {
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.Now },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Sword", Price = 19, CreatedDate = DateTimeOffset.Now },
            new Item { Id = Guid.NewGuid(), Name = "Silver Sword", Price = 29, CreatedDate = DateTimeOffset.Now }
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
        }

        public async Task<Item> GetItemAsync(Guid guid)
        {

            return await Task.FromResult(items.Where(items => items.Id == guid).SingleOrDefault());
        }

        public async Task PostItemAsync(Item item)
        {
            items.Add(item);
            await Task.CompletedTask;
        }

        public async Task PutItemAsync(Item item)
        {
           var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
           items[index] = item;
           await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Item item)
        {
           var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
           items.RemoveAt(index);
           await Task.CompletedTask;
        }
    }
}