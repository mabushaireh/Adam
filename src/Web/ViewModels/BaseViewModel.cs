// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.Web.ViewModels
{
    using System;

    public class BaseViewModel
    {
        public string CreatedBy { get; set; }
        
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}