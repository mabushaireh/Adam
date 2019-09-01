// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 
//TODO: add description
// ======================================

namespace i2fam.Web.ViewModels
{
    using i2fam.DAL.Core;
    using i2fam.DAL.Models;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                   .ForMember(d => d.Roles, map => map.Ignore())
                   .ForMember(d => d.Mobile, map => map.MapFrom(s => s.PhoneNumber));


            CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.PhoneNumber, map => map.MapFrom(s => s.Mobile));

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Mobile, map => map.MapFrom(s => s.PhoneNumber));


            CreateMap<UserEditViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.PhoneNumber, map => map.MapFrom(s => s.Mobile));


            CreateMap<ApplicationUser, UserPatchViewModel>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap()
                .ForMember(d => d.Claims, map => map.Ignore());

            CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionViewModel>()
                .ReverseMap();

            CreateMap<FamilyMember, FamilyMemberViewModel>()
                .ReverseMap();

            CreateMap<FamilyMemberUpdate, FamilyMemberViewModel>()
                .ReverseMap();

                 CreateMap<FamilyMemberUpdate, FamilyMember>()
                .ReverseMap();

            CreateMap<LookupItem, LookupItemViewModel>()
              .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
                .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));
        }
    }
}
