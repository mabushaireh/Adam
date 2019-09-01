using i2fam.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Models
{
    public class FamilyMember : FamilyMemberBase
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<FamilyMember> Children { get; set; }
    }
}
