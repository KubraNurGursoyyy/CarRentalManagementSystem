using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.DataAccess.Abstracts
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        T GetByID(int id);
        bool Insert(T entity);
        bool Update(T entity);
        bool DeleteByID(int id);
    }
}
