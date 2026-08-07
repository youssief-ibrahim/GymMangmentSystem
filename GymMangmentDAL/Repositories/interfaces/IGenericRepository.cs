using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Repositories.interfaces
{
    internal interface IGenericRepository<T> where T : BaseEntity,new()
    {
        IEnumerable<T> GetAll();
        T? GetById(int Id);
        void Add(T member);
        void Update(T member);
        void Delete(T Id);
        void save();
    }
}
