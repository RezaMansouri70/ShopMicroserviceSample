using DiscountMicroservice.Model.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountMicroservice.Infrastructure.Contexts
{
   
    public class DiscountContext : IDiscountContext
    {
        public DiscountContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            DiscountCode = database.GetCollection<DiscountCode>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }

        public IMongoCollection<DiscountCode> DiscountCode { get; }
    }

    public interface IDiscountContext
    {
        IMongoCollection<DiscountCode> DiscountCode { get; }
    }
}
