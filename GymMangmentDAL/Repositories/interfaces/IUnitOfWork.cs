using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Repositories.interfaces
{
    public interface IUnitOfWork
    {
       public ISessionRepository SessionRepository { get; }
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity, new();
        int SaveChanges();
    }
}
