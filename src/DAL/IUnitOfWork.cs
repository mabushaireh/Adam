// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

namespace i2fam.DAL
{
    using i2fam.DAL.Models;
    using i2fam.DAL.Repositories.Interfaces;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        IFamilyMemberRepository FamilyMembers { get; }

        IFamilyMemberUpdateRepository FamilyMemberUpdates { get; }

        ILookupItemRepository LookupItems { get; }

        Task<int> SaveChangesAsync();
    }
}
