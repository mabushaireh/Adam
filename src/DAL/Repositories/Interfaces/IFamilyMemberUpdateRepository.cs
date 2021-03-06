﻿using i2fam.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.DAL.Repositories.Interfaces
{
    public interface IFamilyMemberUpdateRepository
    {
        Task<int> AddFamilyMemberUpdateAsync(FamilyMemberUpdate update);

        Task<FamilyMemberUpdate> GetFamilyMemberUpdateByIdAsync(int id);

        Task UpdateFamilyMemberUpdatesAsync (FamilyMemberUpdate update);
    }
}
