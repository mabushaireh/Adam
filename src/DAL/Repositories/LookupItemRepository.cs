using i2fam.DAL.Models;
using i2fam.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i2fam.DAL.Core;
using Microsoft.EntityFrameworkCore;

namespace i2fam.DAL.Repositories
{
    public class LookupItemRepository : Repository<LookupItem>, ILookupItemRepository
    {
        private ApplicationDbContext appContext => (ApplicationDbContext)this._context;

        public LookupItemRepository(DbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<LookupItem>> GetLookupItemByCategoryAsync(LocaleCategory localeCategory)
        {
            var query = this.appContext.LookupItems.Where(c => c.CategoryId == localeCategory).OrderBy(c => c.Id);

            return await query.ToListAsync();
        }
    }
}
