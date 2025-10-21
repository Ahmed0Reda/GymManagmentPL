using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel input);
        bool UpdateSession(int id, UpdateSessionViewModel input);
        UpdateSessionViewModel? GetSessionToUpdate(int id);
        bool RemoveSession(int id);
    }
}
