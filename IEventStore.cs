using System.Collections;
using System.Collections.Generic;

namespace Citolab.Repository
{
    public interface IEventStore<T>
    {
        void Save(T e);

        IEnumerable<T> Get(int size, int page);
    }
}