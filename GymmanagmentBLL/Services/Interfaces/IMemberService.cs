using GymmanagmentBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Interfaces
{
    public interface IMemberService
    {
        bool CreateMember(CreateMemberViewModel model);
        bool UpdateMemberDetails(int memberId, MemberToUpdateViewModel model);
        bool RemoveMember(int memberId);
        IEnumerable<MemberViewModel> GetAllMembers();
        MemberViewModel? GetMemberDetails(int memberId);
        HealthRecordViewModel? GetMemberHealthRecord(int memberId);
        MemberToUpdateViewModel? GetMemberForUpdate(int memberId); 
    }
}
