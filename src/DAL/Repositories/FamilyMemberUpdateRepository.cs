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
    public class FamilyMemberUpdateRepository : Repository<FamilyMemberUpdate>, IFamilyMemberUpdateRepository
    {
        private ApplicationDbContext appContext => (ApplicationDbContext)this._context;

        public FamilyMemberUpdateRepository(DbContext context) : base(context)
        {
        }

        public async Task<int> AddFamilyMemberUpdateAsync(FamilyMemberUpdate update)
        {
            await this.appContext.FamilyMemberUpdates.AddAsync(update);

            await this.appContext.SaveChangesAsync();

            return update.Id;

        }

        public Task<FamilyMemberUpdate> GetFamilyMemberUpdateByIdAsync(int id)
        {
            var query = this.appContext.FamilyMemberUpdates.Include(c => c.Ref).Where(f => f.Id == id).SingleOrDefaultAsync();

            return query;
        }

        public async Task UpdateFamilyMemberUpdatesAsync(FamilyMemberUpdate update)
        {
            this.appContext.FamilyMemberUpdates.Update(update);

            await this.appContext.SaveChangesAsync();
        }
    }
}
