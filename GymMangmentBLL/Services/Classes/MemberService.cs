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

        //private readonly IGenericRepository<Member> memberReposatoriy;
        //private readonly IPlanRepository planRepository;
        //private readonly IGenericRepository<MemberShip> membershipreposatory;
        //private readonly IGenericRepository<HealthRecord> healthRecordReposatory;
        //private readonly IGenericRepository<MemberSession> memberSessionReposatory;

        //public MemberService(IGenericRepository<Member> memberReposatoriy,IPlanRepository planRepository,IGenericRepository<MemberShip> membershipreposatory,IGenericRepository<HealthRecord>healthRecordReposatory,IGenericRepository<MemberSession>memberSessionReposatory)
        //{
        //    this.memberReposatoriy = memberReposatoriy;
        //    this.planRepository = planRepository;
        //    this.membershipreposatory = membershipreposatory;
        //    this.healthRecordReposatory = healthRecordReposatory;
        //    this.memberSessionReposatory = memberSessionReposatory;
        //}
        private readonly IUnitOfWork unitOfWork;
        public MemberService(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
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
                unitOfWork.GetRepository<Member>().Add(member);
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }

        public IEnumerable<MemberViewModels> GetAllMembers()
        {
            var member = unitOfWork.GetRepository<Member>().GetAll();
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
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
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
            var ActiveMembership = unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == MemberId && ms.Status=="Active").FirstOrDefault();
            if (ActiveMembership is not null)
            {
                ViewModel.MembershipStart = ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEnd = ActiveMembership.EndDate.ToShortDateString();
                var plan = unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;

        }

        public MemberToUpdateViewModel? GetMemberForUpdate(int MemberId)
        {
          var member= unitOfWork.GetRepository<Member>().GetById(MemberId);
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
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);

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
            var membeRepo = unitOfWork.GetRepository<Member>();
            var Member = membeRepo.GetById(MemberId);

            if (Member is null)
                return false;

            var HasActiveMemberSessions = unitOfWork.GetRepository<MemberSession>()
                .GetAll(X => X.MemberId == MemberId &&
                             X.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions)
                return false;

            var memberShipRepo = unitOfWork.GetRepository<MemberShip>();

            var Memberships = memberShipRepo
                .GetAll(X => X.MemberId == MemberId);

            try
            {
                if (Memberships.Any())
                {
                    foreach (var membership in Memberships)
                    {
                        memberShipRepo.Delete(membership);
                    }
                }

                 membeRepo.Delete(Member) ;
                return unitOfWork.SaveChanges() > 0;
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

                var memberRepo = unitOfWork.GetRepository<Member>();

                var member = memberRepo.GetById(MemberId);
                if (member is null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.City = memberToUpdate.City;
                member.Address.City = memberToUpdate.Street;

                memberRepo.Update(member) ;
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        #region private-method
        private bool IsExistMail(string mail)
        {
           return unitOfWork.GetRepository<Member>().GetAll(e => e.Email == mail).Any();
        }
        private bool IsExistPhone(string phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(e => e.Phone == phone).Any();
        }
        #endregion
    }
}
