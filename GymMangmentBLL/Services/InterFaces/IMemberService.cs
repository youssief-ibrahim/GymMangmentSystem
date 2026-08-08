using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmentDAL.Entities;

namespace GymMangmentBLL.Services.InterFaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModels> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMemberViewModel);
        MemberViewModels? GetMemberDetails(int MemberId);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);
        MemberToUpdateViewModel? GetMemberForUpdate(int MemberId);
        bool UpdateMember(int MemberId, MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int MemberId);

    }
}
