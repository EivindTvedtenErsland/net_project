using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Models;

namespace net_project.Api.Api.Repositories
{

    public class MongoDBItemsReposity : ItemsRepositoryInterface
    {
        private readonly IMongoCollection<Item> itemsCollectionMongoDB;
        private const string databaseName = "mongodb";
        private const string collectionName = "items";
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
    
        public MongoDBItemsReposity(IMongoClient mongoClient)
        {
           
            IMongoDatabase mongoDB = mongoClient.GetDatabase(databaseName);
            itemsCollectionMongoDB = mongoDB.GetCollection<Item>(collectionName);
        }
        public async Task<Item> GetItemAsync(Guid guid)
        {
            var filter = filterBuilder.Eq(item => item.Id, guid);
            return await itemsCollectionMongoDB.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollectionMongoDB.Find(new BsonDocument()).ToListAsync();
        }

        public async Task PostItemAsync(Item item)
        {
            await itemsCollectionMongoDB.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollectionMongoDB.DeleteOneAsync(filter);
        }

        public async Task PutItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollectionMongoDB.ReplaceOneAsync(filter, item);
        }
    }
}