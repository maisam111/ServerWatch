using MongoDB.Driver;
using ServerAuth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerAuth.Database;
using MongoDB.Bson;

namespace ServerAuth.Services
{
    public class WatchService
    {
        private MongoDbWrapper _database;

        public WatchService(MongoDbWrapper iDatabase)
        {
            _database = iDatabase;
        }

        public async Task<IEnumerable<Watch>> GetAllAsync()
        {
            var emptyFilter = Builders<Watch>.Filter.Empty;

            IAsyncCursor<Watch> result = await _database.WatchCollection.FindAsync(emptyFilter);
            List<Watch> allDocuments = await result.ToListAsync();

            return allDocuments;
        }

        public async Task<Watch> GetByIdAsync(string WatchId)
        {
            var filter = Builders<Watch>.Filter.Eq("Id", WatchId);

            return await (await _database.WatchCollection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Watch iWatch)
        {
            await _database.WatchCollection.InsertOneAsync(iWatch);
        }
        public async Task<bool> UpdateAsync(Watch iWatch)
        {
            var filter = Builders<Watch>.Filter.Eq("Id", iWatch.Id);
            var result = await _database.WatchCollection.ReplaceOneAsync(filter, iWatch);
            if (result.IsAcknowledged)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(string watchId)

        {
            var filter = Builders<Watch>.Filter.Eq("Id", ObjectId.Parse(watchId));
            var deleteStatus = await _database.WatchCollection.DeleteOneAsync(filter);
            System.Console.WriteLine(deleteStatus);
            System.Console.WriteLine(deleteStatus.ToString());
            if (deleteStatus.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
