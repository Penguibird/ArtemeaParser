using Data.Entities;
// using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Context
{
    public class ArtemeaDb
    {

        private readonly string _connection;
        private readonly string _dbName;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public static readonly string USERS = "users";

        public ArtemeaDb(string connection, string dbName)
        {
            _connection = connection;
            _dbName = dbName;
            _client = new MongoClient(_connection);
            _database = _client.GetDatabase(_dbName);
        }

        public async Task Add<T>(string collectionName, T document)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(document);
        }
        public async Task Delete(string collectionName, Guid id)
        {
            IMongoCollection<BaseEntity> collection = _database.GetCollection<BaseEntity>(collectionName);
            await collection.DeleteOneAsync(element => element.ID == id);
        }
        public async Task<List<T>> GetList<T>(string collectionName)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            return await (await collection.FindAsync(new BsonDocument())).ToListAsync();
        }
        public async Task<T> FindById<T>(string collectionName, Guid id)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            return await (await collection.FindAsync(Builders<T>.Filter.Eq("ID", id))).FirstOrDefaultAsync();
        }
        public async Task<List<T>> FindByAttribute<T, R>(string collectionName, string attributeName, R attribute)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            return await (await collection.FindAsync(Builders<T>.Filter.Eq(attributeName, attribute))).ToListAsync();
        }
        public async Task<List<T>> FindByAttributes<T, R, S>(string collectionName, string attribute1Name, R attribute1, string attribute2Name, S attribute2)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            IList<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();
            filters.Add(Builders<T>.Filter.Eq(attribute1Name, attribute1));
            filters.Add(Builders<T>.Filter.Eq(attribute2Name, attribute2));
            filters.Select(p => Builders<T>.Filter.Eq("_id", p)).Aggregate((p1, p2) => p1 & p2);
            var filterConcat = Builders<T>.Filter.And(filters);
            return await (await collection.FindAsync<T>(filterConcat)).ToListAsync();
        }
        public async Task Replace<T>(string collectionName, Guid id, T newDocument)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            await collection.FindOneAndReplaceAsync<T>(Builders<T>.Filter.Eq("ID", id), newDocument);
        }
    }
}
