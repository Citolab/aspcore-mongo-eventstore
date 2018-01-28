using System;
using Citolab.Mongo.EventStore.Mongo;
using Citolab.Mongo.EventStore.Options;
using Citolab.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace Citolab.Mongo.EventStore.Extensions
{
    public static class Extensions
    {
        public static void AddMongoEventStore<T>(this IServiceCollection services, string databaseName,
            string connectionString, string collectionName) =>
            services.AddEventStore<T>(new MongoEventStoreOptions(databaseName, connectionString, collectionName));

        public static void AddInMemoryEventStore<T>(this IServiceCollection services, string collectionName) =>
            services.AddEventStore<T>(new InMemoryEventStoreOptions(collectionName));

        private static void AddEventStore<T>(this IServiceCollection services, IEventStoreOptions options)
        {
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton(options);
            switch (options)
            {
                case IMongoEventStoreOptions _:
                    {
                        BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
                        services.AddSingleton<IEventStore<T>, MongoEventStore<T>>();
                        break;
                    }
                case IInMemoryEventStoreOptions _:
                    {
                        services.AddSingleton<IEventStore<T>, MongoEventStore<T>>();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported database");
            }
        }

        /// <summary>
        ///     Deep clone using JSON serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toClone"></param>
        /// <returns></returns>
        public static T Clone<T>(this T toClone) where T : class =>
            JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(toClone));        
    }
}
