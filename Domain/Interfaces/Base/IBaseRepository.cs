using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ForecastingSystem.Domain.Interfaces.Base
{
    public interface IBaseRepository<T> where T:class
    {
        /*
            This is where we put all the methods
            that are common for all entities.
         */
 
        IReadOnlyList<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
