// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.DAL.Models
{
    using System;
    using System.Collections.Generic;

    using i2fam.DAL.Models.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public ApplicationUser() {
        }
        public virtual string FriendlyName
        {
            get
            {
                return this.FullName;
            }
        }

        public virtual string FullName
        {
            get
            {
                string fullName = $"{this.FirstName} {this.LastName}";

                return fullName;
            }
        }

        public override string UserName { get => base.Email; set => base.Email= value; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Configuration { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;



        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }



        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        /// <summary>
        /// Demo Navigation property for orders this user has processed
        /// </summary>
    }
}
