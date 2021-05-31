using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models.Interfaces
{
    interface IHttpClientHandler<T>
    {
        Task <List<T>> GetAll(string queryString);
       
       Task <T> Get(int id);

        Task<T> Create(T item);

        Task<T> Delete(int id);

        Task <Dictionary<string, string>> Update(T item);

       

       
        
    }
}
