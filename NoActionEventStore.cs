using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Citolab.Repository;

namespace Citolab.EventStore.Mongo
{
    public class NoActionEventStore<TEvent> : IEventStore<TEvent>
    {
        public List<TEvent> Events { get; set; }
        public NoActionEventStore() => Events = new List<TEvent>();

        public void Save(TEvent e) => Events.Add(e);

        public IEnumerable<TEvent> Get(int size, int page)=> 
            Events.OrderByDescending(x => x)
                  .Skip(size * (page - 1)).Take(size);
        public Task AwaitEvents(int amountOfEvents) =>
            Task.Run(() =>
            {
                var startCount = Events.Count;
                while (Events.Count - startCount < amountOfEvents)
                {
                    Task.Delay(1);
                }
            });        
    }
}