// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
//

// ======================================

import { Component, OnInit, ViewChild, Input, ElementRef } from "@angular/core";
import { fadeInOut } from "../../services/animations";
import { BootstrapTabDirective } from "../../directives/bootstrap-tab.directive";
import { BootstrapToggleDirective } from "../../directives/bootstrap-toggle.directive";

import { AlertService, MessageSeverity } from "../../services/alert.service";
// import { AccountService } from "../../services/account.service";
import { FamilyMemberService } from "../../services/familyMember.service";
import { Utilities } from "../../services/utilities";
import { User } from "../../models/user.model";
import { UserEdit } from "../../models/user-edit.model";
import { Role } from "../../models/role.model";
import { Permission } from "../../models/permission.model";
import { FamilyMember } from "../../models/familyMember.model";
import {
  Gender,
  PhoneType,
  MaritalStatus,
  LocaleCategory,
  Language
} from "../../models/enums";
import { LookupItemService } from "../../services/lookupItem.service";
import { LookupItem } from "../../models/lookupItem.model";
import { AppTranslationService } from "../../services/app-translation.service";
import { ConfigurationService } from "../../services/configuration.service";

@Component({
  selector: "profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"],
  animations: [fadeInOut]
})
export class ProfileComponent implements OnInit {
  //#region fields

  public profile: FamilyMember = new FamilyMember();
  public profileEdit: FamilyMember = new FamilyMember();
  public uniqueId: string = Utilities.uniqueId();

  public isEditMode = false;
  public showValidationErrors = false;
  public isSaving = false;
  public formResetToggle = true;

  public isBasicInfoArActivated = true;
  public isBasicInfoEnActivated = false;
  public isContactInfoActivated = false;
  public isOtherDependentsArActivated = false;
  public isOtherDependentsEnActivated = false;
  public isMiscellaneousActivated = false;

  //NOTE: This is to access enums from Html
  public gender = Gender;
  public phoneType = PhoneType;
  public maritalStatus = MaritalStatus;

  //NOTE: ViewChilds Required because ngIf hides template variables from global scope
  @ViewChild("f") public form;

  @ViewChild("familyId") public familyId: ElementRef;

  @ViewChild("firstName_Ar") public firstName_Ar;

  @ViewChild("parentId") public parentId;

  @ViewChild("motherFirstName_Ar") public motherFirstName_Ar;

  @ViewChild("motherLastName_Ar") public motherLastName_Ar;

  @ViewChild("spouseFirstName_Ar") public spouseFirstName_Ar;

  @ViewChild("spouseLastName_Ar") public spouseLastName_Ar;

  @ViewChild("firstName_En") public firstName_En;

  @ViewChild("familyName_En") public familyName_En;

  @ViewChild("countryId") public countryId;

  @ViewChild("displayPhoneType") public displayPhoneType;

  @ViewChild("genderId") public genderId: BootstrapToggleDirective;

  @ViewChild("maritalStatusSelection")
  public maritalStatusSelection: BootstrapToggleDirective;

  @ViewChild("facebook") public facebook;

  @ViewChild("linkedIn") public linkedIn;

  @ViewChild("website") public website;

  @ViewChild("birthYear") public birthYear;

  @ViewChild("deathYear") public deathYear;

  @Input() public isViewOnly: boolean;

  @Input() public isGeneralEditor = false;

  public changesSavedCallback: () => void;
  public changesFailedCallback: () => void;
  public changesCancelledCallback: () => void;
  public allFamilyMembers: FamilyMember[];

  private gT: any;
  private loadingIndicator: boolean;
  private countriesLookupItems: LookupItem[] = new Array();
  private horoscopesLookupItems: LookupItem[] = new Array();
  private isNewFamilyMember: boolean = false;
  private readonly basicInfoArTab = "basicInfoAr";
  private readonly basicInfoEnTab = "basicInfoEn";
  private readonly contactInfoTab = "contactInfo";
  private readonly otherDependentsArTab = "otherDependentsAr";
  private readonly otherDependentsEnTab = "otherDependentsEn";
  private readonly miscellaneousTab = "miscellaneous";
  activeTab: string = this.basicInfoArTab;

  @ViewChild("tab") private tab: BootstrapTabDirective;

  //#endregion fields

  constructor(
    public configurations: ConfigurationService,
    private alertService: AlertService,
    private translationService: AppTranslationService,
    private lookupItemService: LookupItemService,
    private familyMemberService: FamilyMemberService // private accountService: AccountService
  ) {
    this.gT = (key: string) => this.translationService.getTranslation(key);
  }

  //#region private methods

  private onCountriesLookupLoadSuccessful(lookupItems: LookupItem[]) {
    this.countriesLookupItems = lookupItems;
  }

  private onHoroscopesLookupLoadSuccessful(lookupItems: LookupItem[]) {
    this.horoscopesLookupItems = lookupItems;
  }

  private onDataLoadFailed(error: any) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage(
      "Load Error",
      `Unable to retrieve data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(
        error
      )}"`,
      MessageSeverity.error,
      error
    );
  }

  private setActiveTab(tab: string) {
    this.activeTab = tab.split("#", 2).pop();
  }

  //#endregion private methods

  //#region public methods

  public ngOnInit() {
    this.lookupItemService
      .getLookupItemsByCategory(LocaleCategory.Countries)
      .subscribe(
        lookupItems => this.onCountriesLookupLoadSuccessful(lookupItems),
        error => this.onDataLoadFailed(error)
      );
    this.lookupItemService
      .getLookupItemsByCategory(LocaleCategory.Horoscopes)
      .subscribe(
        lookupItems => this.onHoroscopesLookupLoadSuccessful(lookupItems),
        error => this.onDataLoadFailed(error)
      );
  }

  public getCountryByLocaleId(localeId: number): string {
    if (!localeId || localeId == -1)
      return this.translationService.getTranslation("home.filters.none");

    let currentLang =
      this.configurations.language == "en" ? Language.en_US : Language.ar_JO;

    return this.countriesLookupItems.find(
      c => c.localeId == localeId && c.lang == currentLang
    ).localeString;
  }

  public getHoroscopeByLocaleId(localeId: number): string {
    if (!localeId || localeId == -1)
      return this.translationService.getTranslation("home.filters.none");

    let currentLang =
      this.configurations.language == "en" ? Language.en_US : Language.ar_JO;

    return this.horoscopesLookupItems.find(
      c => c.localeId == localeId && c.lang == currentLang
    ).localeString;
  }

  public onShowTab(event) {
    this.setActiveTab(event.target.hash);

    switch (this.activeTab) {
      case this.basicInfoArTab:
        this.isBasicInfoArActivated = true;
        break;
      case this.basicInfoEnTab:
        this.isBasicInfoEnActivated = true;
        break;
      case this.contactInfoTab:
        this.isContactInfoActivated = true;
        break;
      case this.otherDependentsArTab:
        this.isOtherDependentsArActivated = true;
        break;
      case this.otherDependentsEnTab:
        this.isOtherDependentsEnActivated = true;
        break;
      case this.miscellaneousTab:
        this.isMiscellaneousActivated = true;
        if (this.isEditMode) {
          setTimeout(() => {
            this.genderId.destroy();
            this.maritalStatusSelection.destroy();

            this.genderId.initialize({
              on:
                "<i class='fa fa-male fa-fw' style='color:lightblue'></i> Male",
              off:
                "<i class='fa fa-female fa-fw' style='color:pink'></i> Female"
            });

            this.maritalStatusSelection.initialize({
              on: "Married",
              off: "Not Married"
            });
          });
        }
        break;
      default:
        throw new Error(
          "Selected bootstrap tab is unknown. Selected Tab: " + this.activeTab
        );
    }
  }

  public showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  public edit() {
    if (!this.isGeneralEditor) {
      this.isNewFamilyMember = false;
      this.profileEdit = new FamilyMember();
      Object.assign(this.profileEdit, this.profile);
    } else {
      if (!this.profileEdit) this.profileEdit = new FamilyMember();
    }

    this.isEditMode = true;
    this.showValidationErrors = true;
  }

  public onGenderValueChange(isMale: boolean) {
    // setTimeout(() => {
    //   isMale? this.profileEdit.gender = Gender.Male : this.profileEdit.gender = Gender.Female
    // } );
  }

  public save() {
    this.isSaving = true;
    this.alertService.startLoadingMessage("Saving changes...");

    if (this.isNewFamilyMember) {
      //     this.accountService.newUser(this.userEdit).subscribe(user => this.saveSuccessHelper(user), error => this.saveFailedHelper(error));
    } else {

      // trim strings
      this.profileEdit.city != null ? this.profileEdit.city = this.profileEdit.city.trim() :  null;
      this.profileEdit.displayEmail  != null ? this.profileEdit.displayEmail  = this.profileEdit.displayEmail.trim(): null;
      this.profileEdit.displayPhone  != null ? this.profileEdit.displayPhone = this.profileEdit.displayPhone.trim(): null;
      this.profileEdit.facebook!= null ? this.profileEdit.facebook = this.profileEdit.facebook.trim() : null;
      this.profileEdit.familyId != null ? this.profileEdit.familyId = this.profileEdit.familyId.trim() : null;
      this.profileEdit.familyName_En != null ?  this.profileEdit.familyName_En = this.profileEdit.familyName_En.trim() : null;
      this.profileEdit.femChildrenList_Ar != null ?  this.profileEdit.femChildrenList_Ar = this.profileEdit.femChildrenList_Ar.trim() : null;
      this.profileEdit.femChildrenList_En != null ?  this.profileEdit.femChildrenList_En = this.profileEdit.femChildrenList_En.trim() : null;
      this.profileEdit.firstName_Ar!= null ?  this.profileEdit.firstName_Ar = this.profileEdit.firstName_Ar.trim() : null;
      this.profileEdit.firstName_En != null ?  this.profileEdit.firstName_En = this.profileEdit.firstName_En.trim() : null;
      this.profileEdit.linkedIn != null ?  this.profileEdit.linkedIn = this.profileEdit.linkedIn.trim() : null;
      this.profileEdit.motherFirstName_Ar != null ?  this.profileEdit.motherFirstName_Ar = this.profileEdit.motherFirstName_Ar.trim() : null;
      this.profileEdit.motherFirstName_En != null ?  this.profileEdit.motherFirstName_En = this.profileEdit.motherFirstName_En.trim() : null;
      this.profileEdit.motherLastName_Ar != null ?  this.profileEdit.motherLastName_Ar = this.profileEdit.motherLastName_Ar.trim() : null;
      this.profileEdit.motherLastName_En != null ?  this.profileEdit.motherLastName_En = this.profileEdit.motherLastName_En.trim() : null;
      this.profileEdit.province != null ?  this.profileEdit.province = this.profileEdit.province.trim() : null;
      this.profileEdit.prvPrvSpouseFirstName_Ar != null ?  this.profileEdit.prvPrvSpouseFirstName_Ar = this.profileEdit.prvPrvSpouseFirstName_Ar.trim() : null;
      this.profileEdit.prvPrvSpouseFirstName_En != null ?  this.profileEdit.prvPrvSpouseFirstName_En = this.profileEdit.prvPrvSpouseFirstName_En.trim() : null;
      this.profileEdit.prvPrvSpouseLastName_Ar != null ?  this.profileEdit.prvPrvSpouseLastName_Ar = this.profileEdit.prvPrvSpouseLastName_Ar.trim() : null;
      this.profileEdit.prvPrvSpouseLastName_En != null ?  this.profileEdit.prvPrvSpouseLastName_En = this.profileEdit.prvPrvSpouseLastName_En.trim() : null;
      this.profileEdit.prvSpouseFirstName_Ar != null ?  this.profileEdit.prvSpouseFirstName_Ar = this.profileEdit.prvSpouseFirstName_Ar.trim() : null;
      this.profileEdit.prvSpouseFirstName_En != null ?  this.profileEdit.prvSpouseFirstName_En = this.profileEdit.prvSpouseFirstName_En.trim() : null;
      this.profileEdit.prvSpouseLastName_Ar != null ?  this.profileEdit.prvSpouseLastName_Ar = this.profileEdit.prvSpouseLastName_Ar.trim() : null;
      this.profileEdit.prvSpouseLastName_En != null ?  this.profileEdit.prvSpouseLastName_En = this.profileEdit.prvSpouseLastName_En.trim() : null;
      this.profileEdit.spouseFirstName_Ar != null ?  this.profileEdit.spouseFirstName_Ar = this.profileEdit.spouseFirstName_Ar.trim() : null;
      this.profileEdit.spouseFirstName_En != null ?  this.profileEdit.spouseFirstName_En = this.profileEdit.spouseFirstName_En.trim() : null;
      this.profileEdit.spouseLastName_Ar != null ?  this.profileEdit.spouseLastName_Ar = this.profileEdit.spouseLastName_Ar.trim() : null;
      this.profileEdit.spouseLastName_En != null ?  this.profileEdit.spouseLastName_En = this.profileEdit.spouseLastName_En.trim() : null;
      this.profileEdit.website != null ?  this.profileEdit.website = this.profileEdit.website.trim() : null;

      this.familyMemberService
        .requestUpdateFamilyMember(this.profileEdit)
        .subscribe(
          response => this.saveSuccessHelper(),
          error => this.saveFailedHelper(error)
        );
    }
  }

  public cancel() {
    if (this.isGeneralEditor)
      this.profileEdit = this.profile = new FamilyMember();
    else this.profileEdit = new FamilyMember();

    this.showValidationErrors = false;
    this.resetForm();

    this.alertService.showMessage(
      "Cancelled",
      "Operation cancelled by user",
      MessageSeverity.default
    );
    this.alertService.resetStickyMessage();

    if (!this.isGeneralEditor) this.isEditMode = false;

    if (this.changesCancelledCallback) this.changesCancelledCallback();
  }

  public close() {
    this.profile = this.profileEdit = new FamilyMember();
    this.showValidationErrors = false;
    this.resetForm();
    this.isEditMode = false;

    if (this.changesSavedCallback) this.changesSavedCallback();
  }

  public displayProfile(profile: FamilyMember, allProfiles: FamilyMember[]) {
    this.profile = profile;
    this.allFamilyMembers = allProfiles.filter(x => x.id != this.profile.id);

    this.isEditMode = false;
    this.isSaving = false;
  }

  public resetForm(replace = false) {
    if (!replace) {
      this.form.reset();
    } else {
      this.formResetToggle = false;

      setTimeout(() => {
        this.formResetToggle = true;
      });
    }
  }

  //FIXME: This is only for debuggng
  public getSelected(id: number, countryId: number): boolean {
    console.log(id === countryId);
    return id === countryId;
  }

  public getCountriesList(): LookupItem[] {
    let lang: Language =
      this.configurations.language == "en" ? Language.en_US : Language.ar_JO;

    return this.countriesLookupItems.filter(x => x.lang == lang);
  }

  public getHoroscopesList(): LookupItem[] {
    let lang: Language =
      this.configurations.language == "en" ? Language.en_US : Language.ar_JO;

    return this.horoscopesLookupItems.filter(x => x.lang == lang);
  }
  //#endregion public methods

  //#region Tranlations

  public get basicInfo_ar_familyId_validations_required_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.familyId.validations.required.message"
    );
  }
  public get basicInfo_ar_familyId_validations_required_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.familyId.validations.required.title"
    );
  }

  public get basicInfo_ar_familyId_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.familyId.validations.pattern.message"
    );
  }
  public get basicInfo_ar_familyId_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.familyId.validations.pattern.title"
    );
  }

  public get basicInfo_ar_generation_validations_required_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.generation.validations.required.message"
    );
  }
  public get basicInfo_ar_generation_validations_required_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.generation.validations.required.title"
    );
  }

  public get basicInfo_ar_generation_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.generation.validations.pattern.message"
    );
  }
  public get basicInfo_ar_generation_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.generation.validations.pattern.title"
    );
  }

  public get basicInfo_ar_sequence_validations_required_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.sequence.validations.required.message"
    );
  }
  public get basicInfo_ar_sequence_validations_required_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.sequence.validations.required.title"
    );
  }

  public get basicInfo_ar_sequence_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.sequence.validations.pattern.message"
    );
  }
  public get basicInfo_ar_sequence_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.sequence.validations.pattern.title"
    );
  }

  public get basicInfo_ar_firstName_ar_validations_required_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.firstName_ar.validations.required.message"
    );
  }
  public get basicInfo_ar_firstName_ar_validations_required_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.firstName_ar.validations.required.title"
    );
  }

  public get basicInfo_ar_parent_validations_required_message() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.parent.validations.required.message"
    );
  }
  public get basicInfo_ar_parent_validations_required_title() {
    return this.gT(
      "profileControl.tabs.basicInfo_ar.parent.validations.required.title"
    );
  }

  public get contactInfo_displayPhoneType_validations_required_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayPhoneType.validations.required.message"
    );
  }
  public get contactInfo_displayPhoneType_validations_required_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayPhoneType.validations.required.title"
    );
  }

  public get contactInfo_displayEmail_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayEmail.validations.pattern.message"
    );
  }
  public get contactInfo_displayEmail_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayEmail.validations.pattern.title"
    );
  }

  public get contactInfo_displayPhone_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayPhone.validations.pattern.message"
    );
  }
  public get contactInfo_displayPhone_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.displayPhone.validations.pattern.title"
    );
  }

  public get contactInfo_facebook_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.facebook.validations.pattern.message"
    );
  }
  public get contactInfo_facebook_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.facebook.validations.pattern.title"
    );
  }

  public get contactInfo_linkedIn_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.linkedIn.validations.pattern.message"
    );
  }
  public get contactInfo_linkedIn_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.linkedIn.validations.pattern.title"
    );
  }

  public get contactInfo_website_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.contactInfo.website.validations.pattern.message"
    );
  }
  public get contactInfo_website_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.contactInfo.website.validations.pattern.title"
    );
  }

  public get miscellaneous_birthYear_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.miscellaneous.birthYear.validations.pattern.message"
    );
  }
  public get miscellaneous_birthYear_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.miscellaneous.birthYear.validations.pattern.title"
    );
  }

  public get miscellaneous_deathYear_validations_pattern_title() {
    return this.gT(
      "profileControl.tabs.miscellaneous.deathYear.validations.pattern.message"
    );
  }
  public get miscellaneous_deathYear_validations_pattern_message() {
    return this.gT(
      "profileControl.tabs.miscellaneous.deathYear.validations.pattern.title"
    );
  }
  //#endregion Tranlations

  private saveSuccessHelper(familyMember?: FamilyMember) {
    if (familyMember) Object.assign(this.profileEdit, familyMember);

    this.isSaving = false;
    this.alertService.stopLoadingMessage();
    this.showValidationErrors = false;

    // Object.assign(this.profile, this.profileEdit);
    this.profile = new FamilyMember();

    this.resetForm();

    // if (this.isGeneralEditor) {
    if (this.isNewFamilyMember)
      this.alertService.showMessage(
        "Success",
        `Family member \"${
          this.profileEdit.fullName
        }\" was created successfully`,
        MessageSeverity.success
      );
    else
      this.alertService.showMessage(
        "Success",
        `Changes to Family Member \"${
          this.profileEdit.fullName
        }\" was saved successfully`,
        MessageSeverity.success
      );
    // }

    this.isEditMode = false;

    if (this.changesSavedCallback) this.changesSavedCallback();
  }

  private saveFailedHelper(error: any) {
    this.isSaving = false;
    this.alertService.stopLoadingMessage();
    this.alertService.showStickyMessage(
      "Save Error",
      "The below errors occured whilst saving your changes:",
      MessageSeverity.error,
      error
    );
    this.alertService.showStickyMessage(error, null, MessageSeverity.error);

    if (this.changesFailedCallback) this.changesFailedCallback();
  }

  // private refreshLoggedInUser() {
  //     this.accountService.refreshLoggedInUser()
  //         .subscribe(user => {
  //             this.loadCurrentUserData();
  //         },
  //         error => {
  //             this.alertService.resetStickyMessage();
  //             this.alertService.showStickyMessage("Refresh failed", "An error occured whilst refreshing logged in user information from the server", MessageSeverity.error, error);
  //         });
  // }
}
