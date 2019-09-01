using FluentValidation;
using i2fam.DAL.Core;
using i2fam.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.Web.ViewModels
{
    public class FamilyMemberViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string FamilyId { get; set; }
        public int Generation { get; set; }
        public int Sequence { get; set; }
        public string FirstName_Ar { get; set; }
        public string MotherFirstName_Ar { get; set; }
        public string MotherLastName_Ar { get; set; }
        public string SpouseFirstName_Ar { get; set; }
        public string SpouseLastName_Ar { get; set; }
        public string PrvSpouseFirstName_Ar { get; set; }
        public string PrvSpouseLastName_Ar { get; set; }
        public string PrvPrvSpouseFirstName_Ar { get; set; }
        public string PrvPrvSpouseLastName_Ar { get; set; }
        public string FemChildrenList_Ar { get; set; }
        public string FirstName_En { get; set; }
        public string FamilyName_En { get; set; }
        public string MotherFirstName_En { get; set; }
        public string MotherLastName_En { get; set; }
        public string SpouseFirstName_En { get; set; }
        public string SpouseLastName_En { get; set; }
        public string PrvSpouseFirstName_En { get; set; }
        public string PrvSpouseLastName_En { get; set; }
        public string PrvPrvSpouseFirstName_En { get; set; }
        public string PrvPrvSpouseLastName_En { get; set; }
        public string FemChildrenList_En {get; set;}
        public string City { get; set; }
        public string Province { get; set; }
        public int? CountryId { get; set; }
        public string DisplayEmail { get; set; }
        public string DisplayPhone { get; set; }
        public PhoneType? DisplayPhoneType { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Website { get; set; }
        public int? DeathYear { get; set; }
        public int? BirthYear { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public int? HoroscopeId { get; set; }
        public bool PassedAway {get; set;}
        public bool SpousePassedAway {get; set;}

        public int? ParentId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        //public virtual FamilyMember Parent { get; set; }
        //public virtual ICollection<FamilyMember> Childern { get; set; }
    }

    public class FamilyMemberViewModelValidator : AbstractValidator<FamilyMemberViewModel>
    {
        public FamilyMemberViewModelValidator()
        {
            RuleFor(register => register.FamilyId).NotEmpty().WithMessage("Family Id cannot be empty");
            RuleFor(register => register.FamilyId).Matches("^\\d{1,5}$").WithMessage("Family Id Format is a number of maximum five (5) digits!");

            RuleFor(register => register.FirstName_Ar).NotEmpty().WithMessage("First Name (Arabic) cannot be empty");
            RuleFor(register => register.FirstName_Ar).Length(1, 200).WithMessage("First Name (Arabic) cannot exceed 200 characters");
            RuleFor(register => register.MotherFirstName_Ar).Length(1, 200).WithMessage("Mother First Name (Arabic) cannot exceed 200 characters");
            RuleFor(register => register.MotherLastName_Ar).Length(1, 200).WithMessage("Mother Last Name (Arabic) cannot exceed 200 characters");
            RuleFor(register => register.SpouseFirstName_Ar).Length(1, 200).WithMessage("Spouse First Name (Arabic) cannot exceed 200 characters");
            RuleFor(register => register.SpouseLastName_Ar).Length(1, 200).WithMessage("Spouse Last Name (Arabic) cannot exceed 200 characters");
            RuleFor(register => register.FirstName_En).Length(1, 200).WithMessage("First Name (Latin) cannot exceed 200 characters");
            RuleFor(register => register.FamilyName_En).Length(1, 200).WithMessage("Family Name (Latin) cannot exceed 200 characters");
            RuleFor(register => register.MotherFirstName_En).Length(1, 200).WithMessage("Mother First Name (Latin) cannot exceed 200 characters");
            RuleFor(register => register.MotherLastName_En).Length(1, 200).WithMessage("Mother Last Name (Latin) cannot exceed 200 characters");
            RuleFor(register => register.SpouseFirstName_En).Length(1, 200).WithMessage("Spouse First Name (Latin) cannot exceed 200 characters");
            RuleFor(register => register.SpouseLastName_En).Length(1, 200).WithMessage("Spouse Last Name (Latin) cannot exceed 200 characters");

            RuleFor(register => register.Gender).NotEmpty().WithMessage("Gender Cannot be empty");
        }
    }
}
