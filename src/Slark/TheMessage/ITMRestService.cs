using System;
using System.Threading.Tasks;

namespace TheMessage
{
    public interface ITMRestService<T>
    {
        Task<string> PostAsync(T t);
        Task<T> GetAsync(string Id);
        Task<T> DeleteAsync(string Id);
        Task<string> PutAsync(T t);
    }
}
