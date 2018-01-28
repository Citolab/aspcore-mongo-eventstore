# Citolab EventStore for ASP.NET Core

## IEventStore Usage

IEvent store is a minimalistic event store that has two methods: Save(event) and Get(size, page)

### In-memory database
```C#
services.AddInMemoryEventStore<Event>();
```
### MongoDB

```C#
services.AddMongoEventStore<Event>("MyDatabase", Configuration.GetConnectionString("MongoDB"), "MyEvents");
```

### API

In the example below a service that call the two methods. The Get method will return rows from old to new.

```C#
public class EventStoreService
{
    private readonly IEventStore<Event> _eventStore;

    public EventStoreService(IEventStore<Event> eventStore) => _eventStore = eventStore;

    public void Save(Event e) => _eventStore.Save(e);

    public IEnumerable<Event> GetAll()
    {
        var all = new List<Event>();
        for (var page = 1; page < int.MaxValue; page++)
        {
            var rows = _eventStore.Get(50, page).ToList();
            all.AddRange(rows);
            if (!rows.Any()) break;
        }
        return all;
    }
}
```
