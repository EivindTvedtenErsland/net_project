using System;
using System.Collections.Generic;

using net_project.Api.Api.Repositories;
using net_project.Api.Api.Models;
using System.Threading.Tasks;

namespace net_project.Api.Api.Interfaces
{
    public interface ItemsRepositoryInterface
    {
        Task<Item> GetItemAsync(Guid guid);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task PostItemAsync(Item item);
        Task PutItemAsync(Item item);
        Task DeleteItemAsync(Item item);
    }
}