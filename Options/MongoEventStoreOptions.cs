namespace Citolab.EventStore.Mongo.Options
{
    public class MongoEventStoreOptions: IMongoEventStoreOptions
    {
        public MongoEventStoreOptions(string databaseName, string connectionString, string collectionName)
        {
            DatabaseName = databaseName;
            ConnectionString = connectionString;
            CollectionName = collectionName;
        }

        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string CollectionName { get; set; }
    }
}
