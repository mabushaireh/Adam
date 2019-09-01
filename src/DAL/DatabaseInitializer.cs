// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

namespace i2fam.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using i2fam.DAL.Core;
    using i2fam.DAL.Core.Interfaces;
    using i2fam.DAL.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using i2fam.Core;
    using i2fam.Core.Data;
    using System.Linq;

    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }




    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly IAccountManager accountManager;
        private readonly ILogger logger;


        public DatabaseInitializer(
            ApplicationDbContext context,
            IAccountManager accountManager,
            ILogger<DatabaseInitializer> logger)
        {
            this.accountManager = accountManager;
            this.context = context;
            this.logger = logger;
        }

        public async Task SeedAsync()
        {
            await this.context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await this.context.Users.AnyAsync())
            {
                await this.EnsureRoleAsync(ApplicationRole.AdminRoleName, "Default administrator", ApplicationPermissions.GetAdministrativePermissionValues());
                await this.EnsureRoleAsync(ApplicationRole.UserRoleName, "Default user", ApplicationPermissions.GetStanderdMemberPermissionValues());
                await this.CreateUserAsync("basilsq@live.com", "Bluewow", "Basil", "Qubain", "+962778200028", new string[] { ApplicationRole.AdminRoleName });
            }

            if (!await this.context.FamilyMembers.AnyAsync())
            {
                await this.InitializeFamilyMembers();
            }

            if (!await this.context.LookupItems.AnyAsync())
            {
                await this.CreateCountriesAsync();
                await this.CreateHoroscpesAsync();
            }
        }

        private async Task InitializeFamilyMembers()
        {
            dynamic data = DataUtility.GetFamilyMembers();
            List<Tuple<int, int>> ids = new List<Tuple<int, int>>();


            this.logger.LogTrace("");

            foreach (var item in data)
            {
                try
                {
                    var familyMember = new FamilyMember
                    {
                        FamilyId = item["FamilyId"],
                        Generation = item["Generation"],
                        Sequence = item["Sequence"],
                        FirstName_Ar = item["FirstName_Ar"],
                        MotherFirstName_Ar = item["MotherFirstName_Ar"],
                        MotherLastName_Ar = item["MotherLastName_Ar"],
                        SpouseFirstName_Ar = item["SpouseFirstName_Ar"],
                        SpouseLastName_Ar = item["SpouseLastName_Ar"],
                        PrvSpouseFirstName_Ar = item["PrvSpouseFirstName_Ar"],
                        PrvSpouseLastName_Ar = item["PrvSpouseLastName_Ar"],
                        PrvPrvSpouseFirstName_Ar = item["PrvPrvSpouseFirstName_Ar"],
                        PrvPrvSpouseLastName_Ar = item["PrvPrvSpouseLastName_Ar"],
                        FemChildrenList_Ar = item["FemChildrenList_Ar"],
                        FirstName_En = item["FirstName_En"],
                        FamilyName_En = item["FamilyName_En"],
                        MotherFirstName_En = item["MotherFirstName_En"],
                        MotherLastName_En = item["MotherLastName_En"],
                        SpouseFirstName_En = item["SpouseFirstName_En"],
                        SpouseLastName_En = item["SpouseLastName_En"],
                        PrvSpouseFirstName_En = item["PrvSpouseFirstName_En"],
                        PrvSpouseLastName_En = item["PrvSpouseLastName_En"],
                        PrvPrvSpouseFirstName_En = item["PrvPrvSpouseFirstName_En"],
                        PrvPrvSpouseLastName_En = item["PrvPrvSpouseLastName_En"],
                        FemChildrenList_En = item["FemChildrenList_En"],
                        City = item["City"],
                        Province = item["Province"],
                        CountryId = item["CountryId"],
                        DisplayEmail = item["DisplayEmail"],
                        DisplayPhone = item["DisplayPhone"],
                        DisplayPhoneType = item["DisplayPhoneType"],
                        Facebook = item["Facebook"],
                        LinkedIn = item["LinkedIn"],
                        Website = item["Website"],
                        DeathYear = item["DeathYear"],
                        BirthYear = item["BirthYear"],
                        Gender = item["Gender"],
                        MaritalStatus = item["MaritalStatus"],
                        HoroscopeId = item["HoroscopeId"]
                    };

                    if (item["PassedAway"] != null)
                    {
                        familyMember.PassedAway = item["PassedAway"].ToObject<bool>();
                    }

                    if (item["SpousePassedAway"] != null)
                    {
                        familyMember.SpousePassedAway = item["SpousePassedAway"].ToObject<bool>();
                    }

                    if (item["ParentId"] != null)
                    {
                        int parentId = item["ParentId"];

                        familyMember.ParentId = ids.Find(match: c => c.Item1 == parentId).Item2;

                    }

                    this.logger.LogTrace($"Insert details for: {familyMember.FirstName_Ar}");

                    await this.context.FamilyMembers.AddAsync(familyMember);
                    await this.context.SaveChangesAsync();

                    ids.Add(new Tuple<int, int>(int.Parse(familyMember.FamilyId), familyMember.Id));
                }
                catch (System.Exception ex)
                {
                    this.logger.LogError(ex, "Error while importing Family member ");
                }

            }
        }

        private async Task CreateCountriesAsync()
        {
            var countriesEn = DataUtility.GetCountries("en");
            var coutriesAr = DataUtility.GetCountries("ar");

            var index = 1;

            foreach (var country in countriesEn)
            {
                var newCountryEn = new LookupItem
                {
                    CategoryId = LocaleCategory.Countries,
                    Lang = Language.en_US,
                    LocaleId = index,
                    LocaleString = country.Item2
                };

                var newCountryAr = new LookupItem
                {
                    CategoryId = LocaleCategory.Countries,
                    Lang = Language.ar_JO,
                    LocaleId = index,
                    LocaleString = coutriesAr.FirstOrDefault(c => c.Item1 == country.Item1).Item2
                };

                index++;

                await this.context.LookupItems.AddAsync(newCountryEn);
                await this.context.LookupItems.AddAsync(newCountryAr);
            }

            await this.context.SaveChangesAsync();
        }


        private async Task CreateHoroscpesAsync()
        {
            var horoscpesEn = DataUtility.GetHoroscopes("en");
            var horoscpesAr = DataUtility.GetHoroscopes("ar");

            var index = 1;

            foreach (var horoscope in horoscpesEn)
            {
                var newHoroscopeEn = new LookupItem
                {
                    CategoryId = LocaleCategory.Horoscopes,
                    Lang = Language.en_US,
                    LocaleId = int.Parse(horoscope.Item1),
                    LocaleString = horoscope.Item2
                };

                var newHoroscopeAr = new LookupItem
                {
                    CategoryId = LocaleCategory.Horoscopes,
                    Lang = Language.ar_JO,
                    LocaleId = int.Parse(horoscope.Item1),
                    LocaleString = horoscpesAr.FirstOrDefault(c => c.Item1 == horoscope.Item1).Item2
                };

                index++;

                await this.context.LookupItems.AddAsync(newHoroscopeEn);
                await this.context.LookupItems.AddAsync(newHoroscopeAr);
            }

            await this.context.SaveChangesAsync();
        }

        private async Task EnsureRoleAsync(string roleName, string description, IEnumerable<string> claims)
        {
            if ((await this.accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                var applicationRole = new ApplicationRole(roleName, description);

                var result = await this.accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Item1)
                {
                    throw new Exception(
                          $"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
                }
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(
            string email,
            string password,
            string firstName,
            string lastName,
            string phoneNumber,
            IEnumerable<string> roles)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await this.accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Item1)
            {
                throw new Exception(
                       $"Seeding \"{email}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }


            return applicationUser;
        }
    }
}
