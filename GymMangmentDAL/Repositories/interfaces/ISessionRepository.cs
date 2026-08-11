using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Repositories.interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();
        Session? GetSessionWithTrainerAndCategory(int SessionId);
        int GetCountOfBookedSloute(int id);

    }
}
