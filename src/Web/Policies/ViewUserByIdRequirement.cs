// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.Web.Policies
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using i2fam.DAL.Core;
    using i2fam.Web.Helpers;

    using Microsoft.AspNetCore.Authorization;

    public class ViewUserByIdRequirement : IAuthorizationRequirement
    {

    }


    public class ViewUserByIdHandler : AuthorizationHandler<ViewUserByIdRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewUserByIdRequirement requirement, string targetUserId)
        {
            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageMembers) || this.GetIsSameUser(context.User, targetUserId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }


        private bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
        {
            return Utilities.GetUserId(user) == targetUserId;
        }
    }
}
