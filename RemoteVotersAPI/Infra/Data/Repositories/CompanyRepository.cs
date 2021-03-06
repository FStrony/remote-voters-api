﻿using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using remotevotersapi.Domain.Bases;
using remotevotersapi.Domain.Entities;
using remotevotersapi.Infra.ModelSettings;

namespace remotevotersapi.Infra.Data.Repositories
{
    /// <summary>
    /// Company Repository
    ///
    /// Author: FStrony
    /// </summary>
    public class CompanyRepository : BaseRepository
    {
        /// <value>Table name</value>
        public const string CollectionName = "companies";

        /// <value>DB Connection</value>
        public IMongoCollection<Company> Collection { get; set; }

        /// <value>MongoDB configs</value>
        private readonly IOptions<MongoDBConfig> _mongoDBConfig;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="mongoDBConfig"></param>
        public CompanyRepository(IOptions<MongoDBConfig> mongoDBConfig) : base(mongoDBConfig)
        {
            _mongoDBConfig = mongoDBConfig;
            Collection = database.GetCollection<Company>(CollectionName);
        }

        /// <summary>
        /// Create Company
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task Create(Company record)
        {
            await Collection.InsertOneAsync(record);
        }

        /// <summary>
        /// Delete company by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(ObjectId id)
        {
            await Collection.DeleteOneAsync(Builders<Company>.Filter.Eq(record => record.Id, id));
        }

        /// <summary>
        /// Update Company
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task Update(Company record)
        {
            await Collection.ReplaceOneAsync(x => x.Id.Equals(record.Id), record);
        }

        /// <summary>
        /// Retrieve Company information by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Company</returns>
        public async Task<Company> Retrieve(ObjectId id)
        {
            return await Collection.Find(record => record.Id.Equals(id)).FirstAsync();
        }

        /// <summary>
        /// Retrieve Company by Email and Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Company> Retrieve(string email, string password)
        {
            return await Collection.Find(record => record.Email.Equals(email) && record.Password.Equals(password)).FirstAsync(); 
        }
    }
}
