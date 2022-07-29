using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerAuth.Database;
using ServerAuth.Models;

namespace ServerAuth.Services
{
    public class UserService
    {
        private MongoDbWrapper _database;

        public UserService(MongoDbWrapper iDatabase)
        {
            _database = iDatabase;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var emptyFilter = Builders<User>.Filter.Empty;

            IAsyncCursor<User> result = await _database.UsersCollection.FindAsync(emptyFilter);
            List<User> allDocuments = await result.ToListAsync();

            return allDocuments;
        }

        public async Task<User> GetByIdAsync(string UserId)
        {
            var filter = Builders<User>.Filter.Eq("Id", UserId);

            return await (await _database.UsersCollection.FindAsync(filter)).FirstOrDefaultAsync();
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq("Email", email);

            return await (await _database.UsersCollection.FindAsync(filter)).FirstOrDefaultAsync();
        }
        public async Task<User> AddAsync(User iUser)
        {
            if (iUser == null)
            {
                throw new System.Exception("User is null");
            }

            if (string.IsNullOrWhiteSpace(iUser.Email) || string.IsNullOrWhiteSpace(iUser.Password))
            {
                throw new System.Exception("user email or password is empty");
            }

            var user = await GetByEmailAsync(iUser.Email);
            if (user != null)
            {
                throw new System.Exception("user already exist");
            }

            await _database.UsersCollection.InsertOneAsync(iUser);

            return await GetByEmailAsync(iUser.Email);
        }
        public async Task<bool> UpdateAsync(User iUser)
        {
            var filter = Builders<User>.Filter.Eq("Id", iUser.Id);
            var result = await _database.UsersCollection.ReplaceOneAsync(filter, iUser);
            if (result.IsAcknowledged)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(string UserId)
        {
            var filter = Builders<User>.Filter.Eq("Id", UserId);
            var deleteStatus = await _database.UsersCollection.DeleteOneAsync(filter);
            return (deleteStatus.DeletedCount == 1);        
        }

        public async Task<bool> IsValidCredintialsAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new System.Exception("email or password is empty");
            }
            var user = await GetByEmailAsync(email);
            if (user == null)
            {
                throw new System.Exception("user not found!");
            }
            if (user.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
