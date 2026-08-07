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
    internal class MemberRepository: GenericRepository<Member> , IMemberRepository
    {
        // if Member Reposatory have extra fild of Genaric 
        private readonly GymDbContext context;
        public MemberRepository(GymDbContext context) :base(context)
        {
            this.context = context;
        }
       
        public int Talk(int x)
        {
            throw new NotImplementedException();
        }
    }
}
