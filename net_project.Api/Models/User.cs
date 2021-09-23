using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace net_project.Api.Api.Models
{
     public class User
     {
          [BsonId]
          public Guid Id { get; set; }

          public String FirstName { get; set; }

          public String LastName { get; set; }
          
          public String Biography { get; set; }

          public String ImageUrl { get; set; }

          public decimal Age { get; set; }

          public DateTimeOffset CreatedDate { get; set; }  

          public IList<Post> Posts { get; set; }

          public IList<Comment> Comments { get; set; }   
     }
}