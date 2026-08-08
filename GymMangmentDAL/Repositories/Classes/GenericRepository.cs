using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Data.Contexts;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymMangmentDAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext context;
        public GenericRepository(GymDbContext context)
        {
            this.context = context;
        }
        public int Add(T entity)
        {
            context.Set<T>().Add(entity);
            return context.SaveChanges();
        }

        public int Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return context.SaveChanges();
        }

        public IEnumerable<T> GetAll(Func<T, bool>? condition = null)
        {
            if (condition is null) return context.Set<T>().AsNoTracking().ToList();
            else return context.Set<T>().AsNoTracking().Where(condition).ToList();
        }
 
        public T? GetById(int Id)
        {
            return context.Set<T>().Find(Id);
        }

        public int Update(T entity)
        {
           context.Set<T>().Update(entity);
            return context.SaveChanges();
        }

    }
}
