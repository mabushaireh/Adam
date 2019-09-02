// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

namespace i2fam.DAL
{
    using i2fam.DAL.Models;
    using i2fam.DAL.Repositories;
    using i2fam.DAL.Repositories.Interfaces;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private IFamilyMemberRepository familyMembers;

        private IFamilyMemberUpdateRepository familyMemberUpdates;

        private ILookupItemRepository lookupItems;


        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IFamilyMemberRepository FamilyMembers
        {
            get
            {
                if (this.familyMembers == null) this.familyMembers = new FamilyMemberRepository(this.context);

                return this.familyMembers;
            }
        }


        public ILookupItemRepository LookupItems
        {
            get
            {
                if (this.lookupItems == null) this.lookupItems = new LookupItemRepository(this.context);

                return this.lookupItems;
            }
        }

        public IFamilyMemberUpdateRepository FamilyMemberUpdates
        {
            get
            {
                if (this.familyMemberUpdates == null) this.familyMemberUpdates = new FamilyMemberUpdateRepository(this.context);

                return this.familyMemberUpdates;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }
    }
}
