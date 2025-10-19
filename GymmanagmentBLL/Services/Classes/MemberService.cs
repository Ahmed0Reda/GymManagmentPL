using GymmanagmentBLL.Services.Interfaces;
using GymmanagmentBLL.ViewModels.MemberViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Classes;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel model)
        {
            try
            {
                if (IsEmailExists(model.Email))
                    return false; 
                if (IsPhoneExists(model.Phone))
                    return false;
                var member = new Member
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = model.HealthRecord.Height,
                        Weight = model.HealthRecord.Weight,
                        BloodType = model.HealthRecord.BloodType,
                        Note = model.HealthRecord.Note
                    } 
                };
                _unitOfWork.GetRepository<Member>().Add(member);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll() ?? [];
            if (members is null || !members.Any())
                return [];
            var memberViewModels = members.Select(x => new MemberViewModel
            {
                Id = x.Id,
                Photo = x.Photo,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Gender = x.Gender.ToString(),


            });
            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;
            var memberViewModel = new MemberViewModel
            {
                Id = member.Id,
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = FormatAddress(member.Address),
            };
            var ActiveMembership = _unitOfWork.GetRepository<Membership>().GetAll(x => x.MemberId == memberId && x.Status == "Active").FirstOrDefault();
            if (ActiveMembership is not null)
            {
                var ActivePlan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.planId);
                memberViewModel.PlanName = ActivePlan?.Name;
                memberViewModel.MemberShipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }
            return memberViewModel;

        }

        public MemberToUpdateViewModel? GetMemberForUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;
            var memberToUpdateViewModel = new MemberToUpdateViewModel
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City

            };
            return memberToUpdateViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (memberHealthRecord is null)
                return null;
            var healthRecordViewModel = new HealthRecordViewModel
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note
            }; ;
            return healthRecordViewModel;
        }

        public bool RemoveMember(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return false;
            var activebookings = _unitOfWork.GetRepository<Booking>().GetAll(x => x.MemberId == memberId && x.Session.StartDate > DateTime.UtcNow);
            if (activebookings.Any())
                return false;
            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(x => x.MemberId == memberId);
            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                }
                _unitOfWork.GetRepository<Member>().Delete(member);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int memberId, MemberToUpdateViewModel model)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return false;
            if (IsEmailExists(model.Email))
                return false;
            if (IsPhoneExists(model.Phone))
                return false;
            member.Name = model.Name;
            member.Email = model.Email;
            member.PhoneNumber = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Member>().Update(member);
            return true;

        }

        #region Helper Methods
        private string FormatAddress(Address address)
        {
            if (address is null)
                return "N/A";
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }
        private bool IsEmailExists(string email)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email);
            return existingMember is not null && existingMember.Any();
        }
        private bool IsPhoneExists(string phone)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == phone);
            return existingMember is not null && existingMember.Any();
        }
        #endregion
    }
}
