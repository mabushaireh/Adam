using AutoMapper;
using i2fam.DAL;
using i2fam.Web.Policies;
using i2fam.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i2fam.DAL.Models;
using i2fam.DAL.Core;
using i2fam.Core.Email;
using SendGrid.Helpers.Mail;
using i2fam.Web.Helpers;
using i2fam.DAL.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace i2fam.Web.Controllers
{
    [Route("api/[controller]")]
    public class FamilyMemberController : Controller
    {
        private readonly ILogger logger;
        private IUnitOfWork _unitOfWork;
        private readonly IAccountManager _accountManager;
        private readonly IHostingEnvironment _hostingEnvironment;


        public FamilyMemberController(IHostingEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork, ILogger<AuthorizationController> logger, IAccountManager accountManager)
        {
            this.logger = logger;
            this._unitOfWork = unitOfWork;
            this._accountManager = accountManager;
            this._hostingEnvironment = hostingEnvironment;

        }

        [HttpGet]
        [Produces(typeof(List<FamilyMemberViewModel>))]
        [Authorize(AuthPolicies.ViewMembersPolicy)]
        public async Task<IActionResult> GetAllFamilyMembers()
        {
            var allMembers = await _unitOfWork.FamilyMembers.GetAllFamilyMembersAsync(true);
            return this.Ok(value: Mapper.Map<IEnumerable<FamilyMemberViewModel>>(allMembers));
        }

        [HttpPut("{id}")]
        [Authorize(AuthPolicies.ViewMembersPolicy)]
        public async Task<IActionResult> RequestUpdateFamilyMember(int id, [FromBody] FamilyMemberViewModel familyMember)
        {
            if (ModelState.IsValid)
            {
                if (familyMember == null)
                    return BadRequest($"{nameof(familyMember)} cannot be null");

                if (id != familyMember.Id)
                    return BadRequest("Conflicting family member id in parameter and model data");
            }

            FamilyMemberUpdate familyMemberUpdate = Mapper.Map<FamilyMemberViewModel, FamilyMemberUpdate>(familyMember);
            familyMemberUpdate.Status = UpdateStatus.Pending;
            familyMemberUpdate.Id = 0;
            familyMemberUpdate.RefId = id;

            var newId = await this._unitOfWork.FamilyMemberUpdates.AddFamilyMemberUpdateAsync(familyMemberUpdate);

            var userId = Utilities.GetUserId(this.User);
            ApplicationUser appUser = await this._accountManager.GetUserByIdAsync(userId);

            var recipients = new EmailAddress[] {
                        new EmailAddress {
                            Name = EmailSender.Configuration.AdminName,
                            Email = EmailSender.Configuration.AdminEmail
                        }
                    };

            var webRootPath = $"{this.Request.Scheme}://{this.Request.Host}/api/FamilyMember/updates/";

            //TODO: Get Parnet, Controy, and Horsoscope
            var requestParam = new Dictionary<string, string>
                                   {
                                        { "{requester.Name}", appUser.FirstName + " " + appUser.LastName },
                                        { "{requester.Email}", appUser.Email },
                                        { "{requester.PhoneNumber}", appUser.PhoneNumber },
                                        { "{familyMemberUpdate.FamilyId}", familyMemberUpdate.FamilyId },                                        
                                        { "{familyMemberUpdate.BirthYear}", familyMemberUpdate.BirthYear != null ? familyMemberUpdate.BirthYear.Value.ToString() : "N/A" },
                                        { "{familyMemberUpdate.City}", familyMemberUpdate.City != null ? familyMemberUpdate.City : "" },
                                        { "{familyMemberUpdate.Country}", familyMemberUpdate.CountryId != null ? familyMemberUpdate.CountryId.ToString() : "N/A" },
                                        { "{familyMemberUpdate.DeathYear}", familyMemberUpdate.DeathYear != null ? familyMemberUpdate.DeathYear.ToString() : "N/A" },
                                        { "{familyMemberUpdate.DisplayEmail}", familyMemberUpdate.DisplayEmail != null ? familyMemberUpdate.DisplayEmail : "N/A" },
                                        { "{familyMemberUpdate.DisplayPhone}", familyMemberUpdate.DisplayPhone != null ? familyMemberUpdate.DisplayPhone : "N/A" },
                                        { "{familyMemberUpdate.DisplayPhoneType}", familyMemberUpdate.DisplayPhoneType != null ? familyMemberUpdate.DisplayPhoneType.ToString() : "N/A" },
                                        { "{familyMemberUpdate.Facebook}", familyMemberUpdate.Facebook != null ? familyMemberUpdate.Facebook : "N/A" },
                                        { "{familyMemberUpdate.FamilyName_En}", familyMemberUpdate.FamilyName_En != null ? familyMemberUpdate.FamilyName_En : "N/A" },
                                        { "{familyMemberUpdate.FirstName_Ar}", familyMemberUpdate.FirstName_Ar != null ? familyMemberUpdate.FirstName_Ar : "N/A" },
                                        { "{familyMemberUpdate.FirstName_En}", familyMemberUpdate.FirstName_En != null ? familyMemberUpdate.FirstName_En : "N/A" },
                                        { "{familyMemberUpdate.Gender}",  familyMemberUpdate.Gender.ToString() },
                                        { "{familyMemberUpdate.Generation}", familyMemberUpdate.Generation.ToString() },
                                        { "{familyMemberUpdate.Horoscope}", familyMemberUpdate.HoroscopeId != null ? familyMemberUpdate.HoroscopeId.ToString() : "N/A" },
                                        { "{familyMemberUpdate.LinkedIn}", familyMemberUpdate.LinkedIn != null ? familyMemberUpdate.LinkedIn : "N/A" },
                                        { "{familyMemberUpdate.MaritalStatus}", familyMemberUpdate.MaritalStatus != null ? familyMemberUpdate.MaritalStatus.ToString() : "N/A" },
                                        { "{familyMemberUpdate.MotherFirstName_Ar}", familyMemberUpdate.MotherFirstName_Ar != null ? familyMemberUpdate.MotherFirstName_Ar : "N/A" },
                                        { "{familyMemberUpdate.MotherFirstName_En}", familyMemberUpdate.MotherFirstName_En != null ? familyMemberUpdate.MotherFirstName_En : "N/A" },
                                        { "{familyMemberUpdate.MotherLastName_Ar}", familyMemberUpdate.MotherLastName_Ar != null ? familyMemberUpdate.MotherLastName_Ar : "N/A" },
                                        { "{familyMemberUpdate.MotherLastName_En}", familyMemberUpdate.MotherLastName_En != null ? familyMemberUpdate.MotherLastName_En : "N/A" },
                                        { "{familyMemberUpdate.Parent}", familyMemberUpdate.ParentId != null ? familyMemberUpdate.ParentId.ToString() : "N/A" },
                                        { "{familyMemberUpdate.Province}", familyMemberUpdate.Province != null ? familyMemberUpdate.Province : "N/A" },
                                        { "{familyMemberUpdate.Sequence}", familyMemberUpdate.Sequence.ToString()},
                                        { "{familyMemberUpdate.SpouseFirstName_Ar}", familyMemberUpdate.SpouseFirstName_Ar != null ? familyMemberUpdate.SpouseFirstName_Ar : "N/A" },
                                        { "{familyMemberUpdate.SpouseFirstName_En}", familyMemberUpdate.SpouseFirstName_En != null ? familyMemberUpdate.SpouseFirstName_En : "N/A" },
                                        { "{familyMemberUpdate.SpouseLastName_Ar}", familyMemberUpdate.SpouseLastName_Ar != null ? familyMemberUpdate.SpouseLastName_Ar : "N/A" },
                                        { "{familyMemberUpdate.SpouseLastName_En}", familyMemberUpdate.SpouseLastName_En != null ? familyMemberUpdate.SpouseLastName_En : "N/A" },
                                        { "{familyMemberUpdate.PrvSpouseFirstName_Ar}", familyMemberUpdate.PrvSpouseFirstName_Ar != null ? familyMemberUpdate.PrvSpouseFirstName_Ar : "N/A" },
                                        { "{familyMemberUpdate.PrvSpouseFirstName_En}", familyMemberUpdate.PrvSpouseFirstName_En != null ? familyMemberUpdate.PrvSpouseFirstName_En : "N/A" },
                                        { "{familyMemberUpdate.PrvSpouseLastName_Ar}", familyMemberUpdate.PrvSpouseLastName_Ar != null ? familyMemberUpdate.PrvSpouseLastName_Ar : "N/A" },
                                        { "{familyMemberUpdate.PrvSpouseLastName_En}", familyMemberUpdate.PrvSpouseLastName_En != null ? familyMemberUpdate.PrvSpouseLastName_En : "N/A" },
                                        { "{familyMemberUpdate.PrvPrvSpouseFirstName_Ar}", familyMemberUpdate.PrvPrvSpouseFirstName_Ar != null ? familyMemberUpdate.PrvPrvSpouseFirstName_Ar : "N/A" },
                                        { "{familyMemberUpdate.PrvPrvSpouseFirstName_En}", familyMemberUpdate.PrvPrvSpouseFirstName_En != null ? familyMemberUpdate.PrvPrvSpouseFirstName_En : "N/A" },
                                        { "{familyMemberUpdate.PrvPrvSpouseLastName_Ar}", familyMemberUpdate.PrvPrvSpouseLastName_Ar != null ? familyMemberUpdate.PrvPrvSpouseLastName_Ar : "N/A" },
                                        { "{familyMemberUpdate.PrvPrvSpouseLastName_En}", familyMemberUpdate.PrvPrvSpouseLastName_En != null ? familyMemberUpdate.PrvPrvSpouseLastName_En : "N/A" },
                                        { "{familyMemberUpdate.FemChildrenList_Ar}", familyMemberUpdate.FemChildrenList_Ar != null ? familyMemberUpdate.FemChildrenList_Ar : "N/A" },
                                        { "{familyMemberUpdate.FemChildrenList_En}", familyMemberUpdate.FemChildrenList_En != null ? familyMemberUpdate.FemChildrenList_En : "N/A" },
                                        { "{familyMemberUpdate.PassedAway}", familyMemberUpdate.PassedAway ? "Yes" : "No" },
                                        { "{familyMemberUpdate.SpousePassedAway}", familyMemberUpdate.SpousePassedAway ? "Yes" : "No" },
                                        { "{familyMemberUpdate.Website}", familyMemberUpdate.Website != null ? familyMemberUpdate.Website : "N/A" },
                                        { "{Url.Approve}", $"{webRootPath}approve/{newId}" },
                                        { "{Url.Reject}", $"{webRootPath}reject/{newId}" },
                                   };

            var response = await EmailSender.SendEmailAsync(
                    recipients,
                    subject: "Adam.info Request Family Member Update",
                    content: EmailTemplates.GetFamilyMemberUpdatedEmail(requestParam, "en"),
                    isHtml: true,
                    addBccs: false);

            if (!response.Item1)
                this.logger.LogWarning(new EventId(1, "Email Error"), null, $"Failed to send Email Error: {response.Item2}");

            return NoContent();
        }

        [HttpGet("updates/approve/{id}")]
        public async Task<IActionResult> ApproveRequestUpdateFamilyMember(int id)
        {
            if (ModelState.IsValid)
            {
                if (id < 1)
                    return BadRequest($"{nameof(id)} cannot be less than 1");
            }

            var familyMemberUpdate = await this._unitOfWork.FamilyMemberUpdates.GetFamilyMemberUpdateByIdAsync(id);

            familyMemberUpdate.Status = UpdateStatus.Approved;

            familyMemberUpdate.Ref.BirthYear = familyMemberUpdate.BirthYear;
            familyMemberUpdate.Ref.City = familyMemberUpdate.City;
            familyMemberUpdate.Ref.CountryId = familyMemberUpdate.CountryId;
            familyMemberUpdate.Ref.DeathYear = familyMemberUpdate.DeathYear;
            familyMemberUpdate.Ref.DisplayEmail = familyMemberUpdate.DisplayEmail;
            familyMemberUpdate.Ref.DisplayPhone = familyMemberUpdate.DisplayPhone;
            familyMemberUpdate.Ref.DisplayPhoneType = familyMemberUpdate.DisplayPhoneType;
            familyMemberUpdate.Ref.Facebook = familyMemberUpdate.Facebook;
            familyMemberUpdate.Ref.FamilyId = familyMemberUpdate.FamilyId;
            familyMemberUpdate.Ref.FamilyName_En = familyMemberUpdate.FamilyName_En;
            familyMemberUpdate.Ref.FemChildrenList_Ar = familyMemberUpdate.FemChildrenList_Ar;
            familyMemberUpdate.Ref.FemChildrenList_En = familyMemberUpdate.FemChildrenList_En;
            familyMemberUpdate.Ref.FirstName_Ar = familyMemberUpdate.FirstName_Ar;
            familyMemberUpdate.Ref.FirstName_En = familyMemberUpdate.FirstName_En;
            familyMemberUpdate.Ref.Gender = familyMemberUpdate.Gender;
            familyMemberUpdate.Ref.Generation = familyMemberUpdate.Generation;
            familyMemberUpdate.Ref.HoroscopeId = familyMemberUpdate.HoroscopeId;
            familyMemberUpdate.Ref.LinkedIn = familyMemberUpdate.LinkedIn;
            familyMemberUpdate.Ref.MaritalStatus = familyMemberUpdate.MaritalStatus;
            familyMemberUpdate.Ref.MotherFirstName_Ar = familyMemberUpdate.MotherFirstName_Ar;
            familyMemberUpdate.Ref.MotherFirstName_En = familyMemberUpdate.MotherFirstName_En;
            familyMemberUpdate.Ref.MotherLastName_Ar = familyMemberUpdate.MotherLastName_Ar;
            familyMemberUpdate.Ref.MotherLastName_En = familyMemberUpdate.MotherLastName_En;
            familyMemberUpdate.Ref.PassedAway = familyMemberUpdate.PassedAway;
            familyMemberUpdate.Ref.ParentId = familyMemberUpdate.ParentId;
            familyMemberUpdate.Ref.Province = familyMemberUpdate.Province;

            familyMemberUpdate.Ref.PrvPrvSpouseFirstName_Ar = familyMemberUpdate.PrvPrvSpouseFirstName_Ar;
            familyMemberUpdate.Ref.PrvPrvSpouseFirstName_En = familyMemberUpdate.PrvPrvSpouseFirstName_En;
            familyMemberUpdate.Ref.PrvPrvSpouseLastName_Ar = familyMemberUpdate.PrvPrvSpouseLastName_Ar;
            familyMemberUpdate.Ref.PrvPrvSpouseLastName_En = familyMemberUpdate.PrvPrvSpouseLastName_En;
            familyMemberUpdate.Ref.PrvSpouseFirstName_Ar = familyMemberUpdate.PrvSpouseFirstName_Ar;
            familyMemberUpdate.Ref.PrvSpouseFirstName_En = familyMemberUpdate.PrvSpouseFirstName_En;
            familyMemberUpdate.Ref.PrvSpouseLastName_Ar = familyMemberUpdate.PrvSpouseLastName_Ar;
            familyMemberUpdate.Ref.PrvSpouseLastName_En = familyMemberUpdate.PrvSpouseLastName_En;

            familyMemberUpdate.Ref.Sequence = familyMemberUpdate.Sequence;
            familyMemberUpdate.Ref.SpouseFirstName_Ar = familyMemberUpdate.SpouseFirstName_Ar;
            familyMemberUpdate.Ref.SpouseFirstName_En = familyMemberUpdate.SpouseFirstName_En;
            familyMemberUpdate.Ref.SpouseLastName_Ar = familyMemberUpdate.SpouseLastName_Ar;
            familyMemberUpdate.Ref.SpouseLastName_En = familyMemberUpdate.SpouseLastName_En;
            familyMemberUpdate.Ref.SpousePassedAway = familyMemberUpdate.SpousePassedAway;

            familyMemberUpdate.Ref.Website = familyMemberUpdate.Website;

            await this._unitOfWork.FamilyMemberUpdates.UpdateFamilyMemberUpdatesAsync(familyMemberUpdate);

            return NoContent();
        }

        [HttpGet("updates/reject/{id}")]
        public async Task<IActionResult> RejectRequestUpdateFamilyMember(int id)
        {
            if (ModelState.IsValid)
            {
                if (id < 1)
                    return BadRequest($"{nameof(id)} cannot be less than 1");
            }

            var familyMemberUpdate = await this._unitOfWork.FamilyMemberUpdates.GetFamilyMemberUpdateByIdAsync(id);

            familyMemberUpdate.Status = UpdateStatus.Rejected;

            await this._unitOfWork.FamilyMemberUpdates.UpdateFamilyMemberUpdatesAsync(familyMemberUpdate);


            // Send Email...
            return NoContent();
        }
    }
}
