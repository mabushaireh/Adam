using i2fam.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i2fam.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace i2fam.DAL.Repositories
{
    public class FamilyMemberRepository : Repository<FamilyMember>, IFamilyMemberRepository
    {
        private ApplicationDbContext appContext => (ApplicationDbContext)this._context;

        public FamilyMemberRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FamilyMember>> GetAllFamilyMembersAsync(bool includeChildren)
        {
            var query = this.appContext.FamilyMembers.Include(c => c.Children)
            .OrderBy(c => c.Generation)
            .ThenBy(c => c.ParentId).
            ThenBy( c => c.Sequence);

            return await query.ToListAsync();
        }

        public async Task UpdateFamilyMemberAsync(FamilyMember familyMember)
        {
            this.appContext.Update(familyMember);
            await this.appContext.SaveChangesAsync();
            return;
        }

        public async Task<FamilyMember> GetFamilyMemberByFamilyIdAsync(string familyId)
        {
            return await this.appContext.FamilyMembers.SingleOrDefaultAsync(c => c.FamilyId == familyId);     
        }

        public async Task AddFamilyMemberAsync(FamilyMember familyMember)
        {
            await this.appContext.AddAsync(familyMember);
            await this.appContext.SaveChangesAsync();

        }
    }
}

