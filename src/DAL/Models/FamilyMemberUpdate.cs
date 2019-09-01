using i2fam.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Models
{
    public class FamilyMemberUpdate : FamilyMemberBase
    {
        public int Id { get; set; }
        public UpdateStatus Status { get; set; }
        public int RefId {get; set;}
        public virtual FamilyMember Ref { get; set; }
    }
}
