using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Models;

// TODO: Implement GetCommentOfPost/GetCommentsOfPost

namespace net_project.Api.Api.Repositories
{
     public class MongoDBPostsRepository : PostsRepositoryInterface
     {
          private readonly IMongoCollection<Post> postsCollectionMongoDB;
          private const string databaseName = "mongodb";
          private const string collectionName = "posts";
          private readonly FilterDefinitionBuilder<Post> filterBuilder = Builders<Post>.Filter;

          public MongoDBPostsRepository(IMongoClient mongoClient)
          {

               IMongoDatabase mongoDB = mongoClient.GetDatabase(databaseName);
               postsCollectionMongoDB = mongoDB.GetCollection<Post>(collectionName);
          }
          public async Task<Post> GetPostAsync(Guid guid)
          {
               var filter = filterBuilder.Eq(post => post.Id, guid);
               return await postsCollectionMongoDB.Find(filter).SingleOrDefaultAsync();
          }

          public async Task<IEnumerable<Post>> GetPostsAsync()
          {
               return await postsCollectionMongoDB.Find(new BsonDocument()).ToListAsync();
          }

          public async Task PostPostAsync(Post post)
          {
               await postsCollectionMongoDB.InsertOneAsync(post);
          }

          public async Task DeletePostAsync(Post post)
          {
               var filter = filterBuilder.Eq(existingPost => existingPost.Id, post.Id);
               await postsCollectionMongoDB.DeleteOneAsync(filter);
          }

          public async Task PutPostAsync(Post post)
          {
               var filter = filterBuilder.Eq(existingPost => existingPost.Id, post.Id);
               await postsCollectionMongoDB.ReplaceOneAsync(filter, post);
          }

          public Task<IEnumerable<Comment>> GetCommentsOfPostAsync(Guid guid)
          {
               throw new NotImplementedException();
          }

          public Task<Comment> GetCommentOfPostAsync(Guid guid, string title = "")
          {
               throw new NotImplementedException();
          }
     }
}
