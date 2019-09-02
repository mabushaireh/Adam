// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace i2fam.Web.ViewModels
{
    public class UserEditViewModel : UserViewModel
    {
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Last Name is required"), MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string NewPassword { get; set; }
        new private bool IsLockedOut { get; } //Hide base member
    }
}
