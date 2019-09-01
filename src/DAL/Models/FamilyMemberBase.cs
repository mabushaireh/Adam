using i2fam.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Models
{
    public abstract class FamilyMemberBase : AuditableEntity
    {
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
        public string FemChildrenList_Ar {get; set;}
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
        public bool PassedAway {get; set;}
        public bool SpousePassedAway {get; set;}
        public int? HoroscopeId { get; set; }
        public int? ParentId { get; set; }
        public virtual FamilyMember Parent { get; set; }
        
    }
}
