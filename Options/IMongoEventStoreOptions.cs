namespace Citolab.Mongo.EventStore.Options
{
    public interface IMongoEventStoreOptions: IEventStoreOptions
    {
        string DatabaseName { get; set; }

        string ConnectionString { get; set; }
    }
}
