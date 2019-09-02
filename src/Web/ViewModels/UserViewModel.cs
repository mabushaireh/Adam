// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

using FluentValidation;
using i2fam.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace i2fam.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required"), StringLength(50, ErrorMessage = "First Name must be at most 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), StringLength(50, ErrorMessage = "Last Name must be at most 50 characters")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Mobile is required"), StringLength(14, ErrorMessage = "Mobile must be at most 14 characters"), Phone(ErrorMessage = "Invalid phone number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Configuration { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLockedOut { get; set; }

        public string[] Roles { get; set; }
    }




    ////Todo: ***Using DataAnnotations for validations until Swashbuckle supports FluentValidation***
    //public class UserViewModelValidator : AbstractValidator<UserViewModel>
    //{
    //    public UserViewModelValidator()
    //    {
    //        //Validation logic here
    //        RuleFor(user => user.UserName).NotEmpty().WithMessage("Username cannot be empty");
    //        RuleFor(user => user.Email).EmailAddress().NotEmpty();
    //        RuleFor(user => user.Password).NotEmpty().WithMessage("Password cannot be empty").Length(4, 20);
    //    }
    //}
}
