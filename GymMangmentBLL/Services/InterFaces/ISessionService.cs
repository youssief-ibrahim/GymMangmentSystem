using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangmentBLL.ViewModels.SessionViewModels;

namespace GymMangmentBLL.Services.InterFaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GeTAllSession();
        SessionViewModel? GetSessionById(int SessionId);
        bool CreatedSession(CreateSessionViewModel CreatedSession);
        UpdateSessionViewModel? GetSessionForUpdate(int id);
        bool UpdateSession(int id, UpdateSessionViewModel updateSessionViewModel);
        bool DeleteSession(int id);
    }
}
