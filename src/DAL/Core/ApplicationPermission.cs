// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.DAL.Core
{
    public class ApplicationPermission
    {
        public ApplicationPermission()
        {
        }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            this.Name = name;
            this.Value = value;
            this.GroupName = groupName;
            this.Description = description;
        }

        public static implicit operator string(ApplicationPermission permission) => permission.Value;

        public string Name { get; set; }

        public string Value { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }


        public override string ToString()
        {
            return this.Value;
        }
    }
}