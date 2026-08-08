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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext context;
        public PlanRepository(GymDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Plan> GetAll()
        {
          return  context.Plans.AsNoTracking().ToList();
        }

        public Plan? GetById(int Id)
        {
            return context.Plans.Find(Id);
        }

        public int Update(Plan member)
        {
             context.Plans.Update(member);
            return context.SaveChanges();
        }
      
    }
}
