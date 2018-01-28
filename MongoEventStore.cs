using System;
using System.Collections.Generic;
using Citolab.EventStore.Mongo.Options;
using Citolab.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Citolab.EventStore.Mongo
{
    public class MongoEventStore<TEvent> : IEventStore<TEvent>
    {
        /// <summary>
        ///     The collection that contains events.
        /// </summary>
        protected readonly IMongoCollection<TEvent> Collection;

        /// <summary>
        ///     Logger
        /// </summary>
        protected readonly ILogger Logger;

        public MongoEventStore(ILoggerFactory loggerFactory, IEventStoreOptions options)
        {
            if (!(options is IMongoEventStoreOptions mongoOptions)) throw new Exception("Options should be of type IMongoDatabaseOptions");
            Logger = loggerFactory.CreateLogger(GetType());
            var mongoClientSettings = new MongoClientSettings
            {
                Server = MongoServerAddress.Parse(mongoOptions.ConnectionString)
            };
            try
            {
                var client = new MongoClient(mongoClientSettings);
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrWhiteSpace(environment))
                {
                    environment = "testing";
                }
                var mongoDatabase = client.GetDatabase($"{mongoOptions.DatabaseName}-{environment}");
                Collection = mongoDatabase.GetCollection<TEvent>(options.CollectionName);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(
                    $"Error while connecting to {mongoClientSettings.Server.Host}. Exception: {exception.Message}",
                    exception);
                throw;
            }
        }

        public void Save(TEvent e)
        {
            Collection.InsertOneAsync(e);
        }

        // Get Events from old to new.
        public IEnumerable<TEvent> Get(int size, int page)
        {
            return Collection.Find(_ => true)
                .Skip((page - 1) * size)
                .Limit(size)
                .ToList();
        }
    }
}