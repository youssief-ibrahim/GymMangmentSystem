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
    public class SessionRepository : GenericRepository<Session> ,ISessionRepository
    {
        private readonly GymDbContext Context;

        public SessionRepository(GymDbContext context):base(context)
        {
           Context = context;
        }
        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
           return Context.Sessions.Include(x=>x.SessionTrainer)
                                  .Include(x=>x.SessionCategory)
                                  .ToList();
        }

        public int GetCountOfBookedSloute(int id)
        {
            return Context.MemberSessions.Count(x=>x.SessionId== id);
        }

        public Session? GetSessionWithTrainerAndCategory(int SessionId)
        {
            return Context.Sessions.Include(x => x.SessionTrainer)
                                 .Include(x => x.SessionCategory)
                                 .FirstOrDefault(x=>x.Id==SessionId);
        }
    }
}