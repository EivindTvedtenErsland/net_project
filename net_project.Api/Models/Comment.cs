using System;
using MongoDB.Bson.Serialization.Attributes;

namespace net_project.Api.Api.Models
{
     public class Comment
     {
          [BsonId]
          public Guid Id { get; set; }

          public String Title { get; set; }

          public String Content { get; set; }

          public DateTimeOffset CreatedDate { get; set; } 

          public User User { get; set; }

          public Post Post { get; set; }    
     }
}