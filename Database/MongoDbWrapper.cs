using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ServerAuth.Models;

namespace ServerAuth.Database
{
    public class MongoDbWrapper
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        public IMongoCollection<User> UsersCollection { get; private set; }
        public IMongoCollection<Watch> WatchCollection { get; private set; }

        public MongoDbWrapper(IConfiguration configuration)
        {

            _client = new MongoClient(configuration["mongoDb:connectionString"]);
            _database = _client.GetDatabase(configuration["mongoDb:databaseName"]);
            UsersCollection = _database.GetCollection<User>("users");
            WatchCollection = _database.GetCollection<Watch>("watches");
        }
    }
}
