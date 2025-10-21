﻿using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetSessionsWithTrainerAndCategory();
        Session GetSessionWithTrainerAndCategory(int sessionId);
        int GetCountOfBookedSlots(int sessionId);
    }
}
