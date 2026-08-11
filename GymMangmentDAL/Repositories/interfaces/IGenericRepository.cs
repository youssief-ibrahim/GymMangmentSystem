using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Repositories.interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity,new()
    {
         IEnumerable<T> GetAll(Func<T,bool>? condition=null);
         T? GetById(int Id);
         void Add(T member);
         void Update(T member);
         void Delete(T Id);
    }
}
