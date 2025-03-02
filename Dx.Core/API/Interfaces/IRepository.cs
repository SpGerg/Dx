using System.Collections.Generic;

namespace Dx.Core.API.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> Entities { get; }
    
    void Add(T entity);

    T Get(T entity);
    
    void Update(T entity);
    
    bool Remove(T entity);

    void Clear();
}