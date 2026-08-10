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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories = new();
        private readonly GymDbContext context;
        public UnitOfWork(GymDbContext context)
        {
            this.context = context;
        }
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new()
        {
            var EntityType = typeof(T);
            if (repositories.TryGetValue(EntityType, out var repository))
            {
                return (IGenericRepository<T>)repository;
            }
            var newRepository = new GenericRepository<T>(context);
            repositories[EntityType] = newRepository;
            return newRepository;
        }

        public int SaveChanges()
        {
           return context.SaveChanges();
        }
    }
}
