// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 
//TODO: add description
// ======================================

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860


namespace i2fam.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AspNet.Security.OpenIdConnect.Extensions;
    using AspNet.Security.OpenIdConnect.Primitives;
    using AspNet.Security.OpenIdConnect.Server;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using OpenIddict.Core;

    using i2fam.DAL.Core;
    using i2fam.DAL.Models;
    using i2fam.Web.ViewModels;
    using AutoMapper;
    using i2fam.DAL.Core.Interfaces;
    using i2fam.Core.Email;
    using SendGrid.Helpers.Mail;
    using Microsoft.Extensions.Logging;
    using i2fam.DAL;

    public class AuthorizationController : Controller
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountManager _accountManager;
        private const string GetUserByIdActionName = "GetUserById";
        private const string AddFamilyMemberActionName = "AddFamilyMember";

        private readonly ILogger logger;
        private IUnitOfWork _unitOfWork;




        public AuthorizationController(
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IAccountManager accountManager,
            ILogger<AuthorizationController> logger,
            IUnitOfWork unitOfWork)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _accountManager = accountManager;
            this.logger = logger;
            this._unitOfWork = unitOfWork;
        }


        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByEmailAsync(request.Username) ?? await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Please check that your email and password is correct"
                    });
                }

                // Ensure the user is enabled.
                if (!user.IsEnabled)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user account is disabled"
                    });
                }


                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                // Ensure the user is not already locked out.
                if (result.IsLockedOut)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user account has been suspended"
                    });
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (result.RequiresTwoFactor)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Invalid login procedure"
                    });
                }

                // Ensure the user is allowed to sign in.
                if (result.IsNotAllowed)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in"
                    });
                }

                if (!result.Succeeded)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Please check that your email and password is correct"
                    });
                }



                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // NOTE: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The refresh token is no longer valid"
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in"
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported"
            });
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), OpenIdConnectServerDefaults.AuthenticationScheme);


            //if (!request.IsRefreshTokenGrantType())
            //{
            // Set the list of scopes granted to the client application.
            // NOTE: the offline_access scope must be granted
            // to allow OpenIddict to return a refresh token.
            ticket.SetScopes(new[]
            {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Phone,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));
            //}

            ticket.SetResources(request.GetResources());

            // NOTE: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                    continue;


                var destinations = new List<string> { OpenIdConnectConstants.Destinations.AccessToken };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Subject && ticket.HasScope(OpenIdConnectConstants.Scopes.OpenId)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)) ||
                    (claim.Type == CustomClaimTypes.Permission && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }


                claim.SetDestinations(destinations);
            }


            var identity = principal.Identity as ClaimsIdentity;


            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Profile))
            {
                if (!string.IsNullOrWhiteSpace(user.FullName))
                    identity.AddClaim(CustomClaimTypes.FullName, user.FullName, OpenIdConnectConstants.Destinations.IdentityToken);

                if (!string.IsNullOrWhiteSpace(user.FirstName))
                    identity.AddClaim(CustomClaimTypes.FirstName, user.FirstName, OpenIdConnectConstants.Destinations.IdentityToken);

                if (!string.IsNullOrWhiteSpace(user.LastName))
                    identity.AddClaim(CustomClaimTypes.LastName, user.LastName, OpenIdConnectConstants.Destinations.IdentityToken);

                if (!string.IsNullOrWhiteSpace(user.Configuration))
                    identity.AddClaim(CustomClaimTypes.Configuration, user.Configuration, OpenIdConnectConstants.Destinations.IdentityToken);
            }

            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Email))
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    identity.AddClaim(CustomClaimTypes.Email, user.Email, OpenIdConnectConstants.Destinations.IdentityToken);
            }

            if (ticket.HasScope(OpenIdConnectConstants.Scopes.Phone))
            {
                if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                    identity.AddClaim(CustomClaimTypes.Mobile, user.PhoneNumber, OpenIdConnectConstants.Destinations.IdentityToken);
            }


            return ticket;
        }

        [HttpPost("~/connect/signup")]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserEditViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                ApplicationUser appUser = Mapper.Map<ApplicationUser>(user);


                var result = await _accountManager.CreateUserAsync(appUser, new string[] { ApplicationRole.UserRoleName }, user.NewPassword);

                if (result.Item1)
                {
                    UserViewModel userVM = await GetUserViewModelHelper(appUser.Id);

                    var recipients = new EmailAddress[] {
                        new EmailAddress {
                            Name = userVM.FirstName + " " + userVM.LastName,
                            Email = userVM.Email
                        }
                    };

                    var requestParam = new Dictionary<string, string>
                                   {
                                       { "{email}", userVM.Email }
                                   };

                    var response = await EmailSender.SendEmailAsync(
                    recipients,
                    subject: "Qubain.info Account Information",
                    content: EmailTemplates.GetAccountCreatedEmail(requestParam, "en"),
                    isHtml: true,
                    addBccs: true);

                    if (!response.Item1)
                        this.logger.LogWarning(new EventId(1, "Email Error"), null, $"Failed to send Email Error: {response.Item2}");

                    return CreatedAtAction(GetUserByIdActionName, new { id = userVM.Id }, userVM);
                }

                AddErrors(result.Item2);
            }

            return BadRequest(ModelState);
        }


        // TODO: this is not secure and will be replaced later!!!.
        [HttpPost("~/connect/add")]
        [HttpPost("familyMember")]
        public async Task<IActionResult> AddFamilyMember([FromBody] string familyId,
        int generation,
        int sequence,
        string firstName_Ar,
        int gender,
        string parentFamilyId)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(familyId))
                    return BadRequest($"{nameof(familyId)} cannot be empty");

                if (string.IsNullOrWhiteSpace(firstName_Ar))
                    return BadRequest($"{nameof(firstName_Ar)} cannot be empty");

                if (generation < 0)
                    return BadRequest($"{nameof(generation)} cannot be less than zero");

                if (sequence < 1)
                    return BadRequest($"{nameof(sequence)} cannot be less than 1");

                if (gender != 1 && gender != 2)
                    return BadRequest($"{nameof(gender)} can be either 1 (female) or 2 (male)");

                if (string.IsNullOrWhiteSpace(parentFamilyId))
                    return BadRequest($"{nameof(parentFamilyId)} cannot be empty");

                var parentId = 0;

                if (!int.TryParse(parentFamilyId, out parentId))
                    return BadRequest($"{nameof(parentFamilyId)} is not a valid FamilyId Format");

                if (parentId < 0)
                    return BadRequest($"{nameof(parentId)} cannot be less than zero");

                var familyMember = new FamilyMember
                {
                    FamilyId = familyId,
                    Generation = generation,
                    Sequence = sequence,
                    FirstName_Ar = firstName_Ar,
                    Gender = (Gender)gender
                };

                var parent = await this._unitOfWork.FamilyMembers.GetFamilyMemberByFamilyIdAsync(parentFamilyId);

                if (parent is null)
                {
                    AddError($"parent family Id {parentFamilyId} is not found!");
                    return BadRequest(ModelState);
                }
                
                familyMember.ParentId = parent.Id;

                await this._unitOfWork.FamilyMembers.AddFamilyMemberAsync(familyMember);

               
                return CreatedAtAction(AddFamilyMemberActionName, new { id = familyMember.Id }, familyMember);
            }

            return BadRequest(ModelState);
        }

        private async Task<UserViewModel> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _accountManager.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = Mapper.Map<UserViewModel>(userAndRoles.Item1);
            userVM.Roles = userAndRoles.Item2;

            return userVM;
        }

        private void AddErrors(IdentityResult result)
        {
            AddErrors(result.Errors.Select(e => e.Description));
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private void AddError(string error)
        {

            ModelState.AddModelError(string.Empty, error);

        }
    }
}
