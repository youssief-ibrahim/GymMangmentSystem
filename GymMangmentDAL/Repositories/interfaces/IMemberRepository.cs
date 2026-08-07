using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.Classes;

namespace GymMangmentDAL.Repositories.interfaces
{
    internal interface IMemberRepository: IGenericRepository<Member>
    {
        int Talk(int x);
    }
}
