using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourProjectWebApp.Models;

namespace YourProjectWebApp.Interfaces
{
    public interface IEntityApiRest
    {

        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<T>> GetOne<T>();


    }
}
