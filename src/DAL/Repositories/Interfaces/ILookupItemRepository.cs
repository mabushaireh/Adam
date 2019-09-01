using i2fam.DAL.Core;
using i2fam.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Repositories.Interfaces
{
    public interface ILookupItemRepository
    {
        Task<IEnumerable<LookupItem>> GetLookupItemByCategoryAsync(LocaleCategory localeCategory);

    }
}
