using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Repositories.interfaces
{
    internal interface IPlanRepository
    {
        IEnumerable<Plan> GetAll();
        Plan? GetById(int Id);
        void Update(Plan member);
        void Save();
    }
}
