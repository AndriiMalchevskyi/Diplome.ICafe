using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace ICafe.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T item);
        Task<T> Get(T item);
        Task<ICollection<T>> Get(IFilter filter);
        Task<T> Update(T item);
        Task<T> Delete(T item);
    }
}
