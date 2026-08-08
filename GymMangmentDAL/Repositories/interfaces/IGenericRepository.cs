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
        public IEnumerable<T> GetAll(Func<T,bool>? condition=null);
        public T? GetById(int Id);
        public int Add(T member);
        public int Update(T member);
        public int Delete(T Id);
    }
}
