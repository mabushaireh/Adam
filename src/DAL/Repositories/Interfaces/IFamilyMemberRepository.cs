using i2fam.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Repositories.Interfaces
{
    public interface IFamilyMemberRepository
    {
        Task<IEnumerable<FamilyMember>> GetAllFamilyMembersAsync(bool includeChildren);

        Task<FamilyMember> GetFamilyMemberByFamilyIdAsync(string familyId);
        Task UpdateFamilyMemberAsync(FamilyMember familyMember);
        Task AddFamilyMemberAsync(FamilyMember familyMember);
    }
}
