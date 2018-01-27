namespace Citolab.Mongo.EventStore.Options
{
    public class InMemoryEventStoreOptions: IInMemoryEventStoreOptions
    {
        public InMemoryEventStoreOptions(string collectionName)
        {
            CollectionName = collectionName;
        }
        public string CollectionName { get; set; }
    }
}
