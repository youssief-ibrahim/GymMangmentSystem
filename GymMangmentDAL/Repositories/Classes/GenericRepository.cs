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
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext context;
        public GenericRepository(GymDbContext context)
        {
            this.context = context;
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
           return context.Set<T>().AsNoTracking().ToList();
        }

        public T? GetById(int Id)
        {
            return context.Set<T>().Find(Id);
        }

        public void Update(T entity)
        {
           context.Set<T>().Update(entity);
        }
        public void save()
        {
            context.SaveChanges();
        }

    }
}
