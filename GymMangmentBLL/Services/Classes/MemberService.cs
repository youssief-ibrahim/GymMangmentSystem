using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> memberReposatoriy;
        private readonly IPlanRepository planRepository;
        private readonly IGenericRepository<MemberShip> membershipreposatory;
        private readonly IGenericRepository<HealthRecord> healthRecordReposatory;
        private readonly IGenericRepository<MemberSession> memberSessionReposatory;

        public MemberService(IGenericRepository<Member> memberReposatoriy,IPlanRepository planRepository,IGenericRepository<MemberShip> membershipreposatory,IGenericRepository<HealthRecord>healthRecordReposatory,IGenericRepository<MemberSession>memberSessionReposatory)
        {
            this.memberReposatoriy = memberReposatoriy;
            this.planRepository = planRepository;
            this.membershipreposatory = membershipreposatory;
            this.healthRecordReposatory = healthRecordReposatory;
            this.memberSessionReposatory = memberSessionReposatory;
        }

        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                //var Emailexist = memberReposatoriy.GetAll(e => e.Email == createMemberViewModel.Email).Any();
                //var Phoneexist = memberReposatoriy.GetAll(e => e.Phone == createMemberViewModel.Phone).Any();

                //if (Emailexist || Phoneexist) return false;

                if (IsExistMail(createMemberViewModel.Email) || IsExistPhone(createMemberViewModel.Phone)) return false;

                var member = new Member()
                {
                    Name = createMemberViewModel.Name,
                    Email = createMemberViewModel.Email,
                    Phone = createMemberViewModel.Phone,
                    Gender = createMemberViewModel.Gender,
                    DateOfBirth = createMemberViewModel.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMemberViewModel.BuildingNumber,
                        City = createMemberViewModel.City,
                        Street = createMemberViewModel.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMemberViewModel.HealthRecordViewModel.Height,
                        Weight = createMemberViewModel.HealthRecordViewModel.Weight,
                        BloodType = createMemberViewModel.HealthRecordViewModel.BloodType,
                        Note = createMemberViewModel.HealthRecordViewModel.Note
                    }
                };
              return memberReposatoriy.Add(member)>0;
                
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }

        public IEnumerable<MemberViewModels> GetAllMembers()
        {
            var member = memberReposatoriy.GetAll();
            if (member is null || !member.Any()) return [];
            var MemberViewModels = member.Select(x => new MemberViewModels
            {
                Email = x.Email,
                Gender = x.Gender.ToString(),
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Photo = x.Photo,
            });
            return MemberViewModels;
        }

        public MemberViewModels? GetMemberDetails(int MemberId)
        {
            var member = memberReposatoriy.GetById(MemberId);
            if (member is null)  return null;
            var ViewModel = new MemberViewModels()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
                Photo = member.Photo,
            };
            var ActiveMembership = membershipreposatory.GetAll(ms => ms.MemberId == MemberId && ms.Status=="Active").FirstOrDefault();
            if (ActiveMembership is not null)
            {
                ViewModel.MembershipStart = ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEnd = ActiveMembership.EndDate.ToShortDateString();
                var plan =planRepository.GetById(ActiveMembership.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;

        }

        public MemberToUpdateViewModel? GetMemberForUpdate(int MemberId)
        {
          var member=memberReposatoriy.GetById(MemberId);
            if (member is null) return null;
            var ViewModel = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber=member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };
            return ViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord =healthRecordReposatory.GetById(MemberId);

            if (MemberHealthRecord is null) return null;

            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Note = MemberHealthRecord.Note,
                Weight = MemberHealthRecord.Weight
            };
        }

        public bool RemoveMember(int MemberId)
        {
            var Member = memberReposatoriy.GetById(MemberId);

            if (Member is null)
                return false;

            var HasActiveMemberSessions = memberSessionReposatory
                .GetAll(X => X.MemberId == MemberId &&
                             X.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions)
                return false;

            var Memberships = membershipreposatory
                .GetAll(X => X.MemberId == MemberId);

            try
            {
                if (Memberships.Any())
                {
                    foreach (var membership in Memberships)
                    {
                        membershipreposatory.Delete(membership);
                    }
                }

                return memberReposatoriy.Delete(Member) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMember(int MemberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {


                //var Emailexist = memberReposatoriy.GetAll(e => e.Email == memberToUpdate.Email).Any();
                //var Phoneexist = memberReposatoriy.GetAll(e => e.Phone == memberToUpdate.Phone).Any();
                //if (Emailexist || Phoneexist) return false;

                if (IsExistMail(memberToUpdate.Email) || IsExistPhone(memberToUpdate.Phone)) return false;

                var member = memberReposatoriy.GetById(MemberId);
                if (member is null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.City = memberToUpdate.City;
                member.Address.City = memberToUpdate.Street;

                return memberReposatoriy.Update(member) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        #region private-method
        private bool IsExistMail(string mail)
        {
           return memberReposatoriy.GetAll(e => e.Email == mail).Any();
        }
        private bool IsExistPhone(string phone)
        {
            return memberReposatoriy.GetAll(e => e.Phone == phone).Any();
        }
        #endregion
    }
}
