// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.DAL.Core
{
    public enum Gender
    {
        None,
        Female,
        Male
    }

    public enum PhoneType
    {
        Mobile,
        Business,
        Home,
    }

    public enum MaritalStatus
    {
        None,
        Married,
        NotMarried
    }

    public enum LocaleCategory
    {
        Countries,
        Horoscopes,
    }
    public enum Language
    {
        en_US,
        ar_JO,
    }

    public enum UpdateStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
