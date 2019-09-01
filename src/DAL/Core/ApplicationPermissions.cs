// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 
//TODO: add description
// ======================================

namespace i2fam.DAL.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Microsoft.Extensions.Primitives;

    public static class ApplicationPermissions
    {
        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions;

        private const string MembersPermissionGroupName = "Member Permissions";
        public static ApplicationPermission ViewMembers = new ApplicationPermission("View Members", "members.view", MembersPermissionGroupName, "Permission to view other family members profile details");
        public static ApplicationPermission ManageMembers = new ApplicationPermission("Manage Members", "members.manage", MembersPermissionGroupName, "Permission to create, delete and modify other family members account details");

        private const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        
        static ApplicationPermissions()
        {
            var allPermissions = new List<ApplicationPermission>()
            {
                ViewMembers,
                ManageMembers,

                ViewRoles,
                ManageRoles,
                AssignRoles,
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.FirstOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.FirstOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static IEnumerable<string> GetAdministrativePermissionValues()
            => new string[] { ViewMembers, ManageMembers, ViewRoles, ManageRoles, AssignRoles};

        public static IEnumerable<string> GetStanderdMemberPermissionValues()
            => new string[] { ViewMembers };
    }
}
