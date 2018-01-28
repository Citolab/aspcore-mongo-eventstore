namespace Citolab.EventStore.Mongo.Options
{
    public interface IMongoEventStoreOptions: IEventStoreOptions
    {
        string DatabaseName { get; set; }

        string ConnectionString { get; set; }
    }
}
