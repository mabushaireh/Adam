// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
//

// ======================================

import {
  Component,
  ViewChild,
  TemplateRef,
  OnInit,
  AfterViewInit,
  ViewEncapsulation
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";

import { fadeInOut } from "../../services/animations";
import { Utilities } from "../../services/utilities";
import { FamilyMember } from "../../models/familyMember.model";
import { ConfigurationService } from "../../services/configuration.service";
import { AppTranslationService } from "../../services/app-translation.service";
import { FamilyMemberService } from "../../services/familyMember.service";
import { LookupItemService } from "../../services/lookupItem.service";

import {
  Gender,
  LocaleCategory,
  Language,
  PhoneType,
  MaritalStatus
} from "../../models/enums";
import { LookupItem } from "../../models/lookupItem.model";

import {
  AlertService,
  DialogType,
  MessageSeverity
} from "../../services/alert.service";
import { Filter } from "../../models/filter.model";
import { ProfileComponent } from "../controls/profile.component";

declare var getorgchart: any;

//var getorgchart: any = require('../../assets/scripts/getorgchart.js');
var orgChart: any = require("../../assets/scripts/getorgchart.js");

@Component({
  selector: "home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
  animations: [fadeInOut],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit, AfterViewInit {
  columns: any[] = [];
  rows: FamilyMember[] = [];
  rowsCache: FamilyMember[] = [];
  loadingIndicator: boolean;
  public gender = Gender;
  public phoneType = PhoneType;
  public maritalStatus = MaritalStatus;
  languageChangedSubscription: any;
  countries: number[] = new Array();
  countriesLookupItems: LookupItem[] = new Array();
  filters: Filter[] = new Array();
  genders: Gender[] = new Array();
  searchKeyword: string;
  editingFamilyMember: { id: number; fullName: string };
  editedFamilyMember: FamilyMember;
  sourceFamilyMember: FamilyMember;
  expandedRow: FamilyMember;
  isTree: boolean = false;

  @ViewChild("myTable") table: any;
  @ViewChild("myTree") myTree: any;

  @ViewChild("myDetailRow") myDetailRow: any;

  @ViewChild("actionsTemplate") actionsTemplate: TemplateRef<any>;

  @ViewChild("fullNameTemplate") fullNameTemplate: TemplateRef<any>;

  @ViewChild("genderTemplate") genderTemplate: TemplateRef<any>;

  @ViewChild("editorModal") editorModal: ModalDirective;

  @ViewChild("profileEditor") profileEditor: ProfileComponent;

  constructor(
    public configurations: ConfigurationService,
    private translationService: AppTranslationService,
    private alertService: AlertService,
    private familyMemberService: FamilyMemberService,
    private lookupItemService: LookupItemService
  ) {}

  ngOnInit() {
    this.lookupItemService
      .getLookupItemsByCategory(LocaleCategory.Countries)
      .subscribe(
        lookupItems => this.onCountriesLookupLoadSuccessful(lookupItems),
        error => this.onDataLoadFailed(error)
      );
  }

  setupDataGrid(loadData: boolean = true) {
    if (loadData) {
      this.loadData();
    }
  }

  viewProfile(row: FamilyMember) {
    this.editingFamilyMember = { id: row.id, fullName: row.fullName };
    this.sourceFamilyMember = row;
    this.profileEditor.displayProfile(row, this.rowsCache);
    this.editorModal.show();
  }

  loadData() {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.familyMemberService
      .getFamilyMembers()
      .subscribe(
        familyMmebers => this.onDataLoadSuccessful(familyMmebers),
        error => this.onDataLoadFailed(error)
      );
  }

  onDataLoadSuccessful(familyMembers: FamilyMember[]) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    familyMembers.forEach((member, index, members) => {
      // Set parent
      member.parent = members.find(c => c.id == member.parentId);

      // Create Countires Array
      if (!member.countryId) member.countryId = -1;

      if (!member.horoscopeId) member.horoscopeId = -1;
      this.AddFullName(member);

      this.addToCountiresArray(member.countryId);
      this.addToGendersArray(member.gender);

    });

    this.rowsCache = [...familyMembers];
    this.rows = familyMembers;

    this.refreshTree(this.rows);
  }

  private AddFullName(member: FamilyMember) {
    let fullName: string = "";

    if (member.firstName_En) {
      fullName =
        member.firstName_En +
        // (member.parent
        //   ? " " + (member.parent.firstName_En ? member.parent.firstName_En : "")
        //   : "") +
        (member.familyName_En ? " " + member.familyName_En : "");
      fullName += " / ";
    }

    fullName +=
      member.firstName_Ar +
      (member.parent ? " " + member.parent.firstName_Ar : "");

    (<any>member).fullName = fullName;
  }

  reset(){
    this.filters.forEach (f => f.source.checked = false);
    this.filters = [];
    this.rows = this.rowsCache;
  }
  
  refreshTree(rows: FamilyMember[]) {
    if (this.isTree) {
      let data: any[] = new Array();

      rows
        .sort((a, b) => {
          return b.sequence - a.sequence;
        })
        .forEach(r => {
          let f = {
            id: r.id,
            parentId: r.parentId,
            firstName_Ar: r.firstName_Ar,
            gender: r.gender
          };
          data.push(f);
        });

      orgChart.init(
        this.myTree.nativeElement,
        data,
        this.renderNodeEventHandler
      );
    }
  }

  renderNodeEventHandler(sender: any, args: any) {
    var gender = args.node.data["gender"];
    var color;

    if (gender == 1) {
      color = "#DC4FAD";
    } else {
      color = "#0072C6";
    }

    args.content[1] = args.content[1].replace(
      "path",
      "path style='fill: " + color + "; stroke: " + color + ";'"
    );
    args.content[2] = args.content[2].replace(
      "text",
      "text style='font-size: 105;'"
    );
  }

  onCountriesLookupLoadSuccessful(lookupItems: LookupItem[]) {
    this.countriesLookupItems = lookupItems;
    this.setupDataGrid(true);
  }

  onDataLoadFailed(error: any) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage(
      "Load Error",
      `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(
        error
      )}"`,
      MessageSeverity.error,
      error
    );
  }

  ngAfterViewInit() {
    this.profileEditor.changesSavedCallback = () => {
      this.editorModal.hide();
    };
  }

  onEditorModalHidden() {
    this.profileEditor.resetForm(true);
  }

  onSearchChanged(value: string) {
    this.searchKeyword = value;
    if (this.filters.length > 0) {
      this.rows = this.rowsCache
        .filter(r => this.isFilter(r, "country"))
          .filter(r => this.isFilter(r, "gender"))
         // TODO: Move the search array to lobal const
        .filter(r =>
            Utilities.searchArray(this.searchKeyword, false, [r.firstName_Ar,
            r.parent == null ? null : r.parent.firstName_Ar,
            r.motherFirstName_Ar, 
            r.motherLastName_Ar, 
            r.spouseFirstName_Ar,
            r.spouseLastName_Ar,
            r.femChildrenList_En,
            r.firstName_En,
            r.familyName_En,
            r.parent  == null ? null : r.parent.firstName_En,
            r.motherFirstName_En, 
            r.motherLastName_En, 
            r.spouseFirstName_En, 
            r.spouseLastName_En,
            r.femChildrenList_En
            ])
        );
    } else {
      this.rows = this.rowsCache.filter(r =>
          Utilities.searchArray(this.searchKeyword, false, [r.firstName_Ar,
              r.parent  == null ? null : r.parent.firstName_Ar,
          r.motherFirstName_Ar,
          r.motherLastName_Ar,
          r.spouseFirstName_Ar,
          r.spouseLastName_Ar,
          r.femChildrenList_En,
          r.firstName_En,
          r.familyName_En,
          r.parent  == null ? null : r.parent.firstName_En,
          r.motherFirstName_En,
          r.motherLastName_En,
          r.spouseFirstName_En,
          r.spouseLastName_En,
          r.femChildrenList_En
          ])
      );
    }

    this.refreshTree(this.rows);
  }

  isNotEmpty(value: string) {
    if (value) {
      return true;
    } else {
      return false;
    }
  }

  addToCountiresArray(countryId?: number) {
    //Check if exists in the array
    let exists = this.countries.indexOf(countryId) > -1;

    if (!exists) {
      this.countries.push(countryId);
    }
  }

  addToGendersArray(gender?: Gender) {
    //Check if exists in the array
    let exists = this.genders.indexOf(gender) > -1;

    if (!exists) {
      this.genders.push(gender);
    }
  }

  getCountryByLocaleId(localeId: number): string {
    if (localeId == -1)
      return this.translationService.getTranslation("home.filters.none");

    let currentLang =
      this.configurations.language == "en" ? Language.en_US : Language.ar_JO;

    return this.countriesLookupItems.find(
      c => c.localeId == localeId && c.lang == currentLang
    ).localeString;
  }

  handleFilterBy(event, source: string) {
    var checkbox = event.target;
    var action = checkbox.checked ? "add" : "remove";
    var filterValue = +checkbox.value;
    this.filterBy(action, checkbox, source, filterValue);
  }

  filterBy(action: string, checkbox: any, source: string, filterValue: number)
  {
    if (action == "remove") {
      this.filters = this.filters.filter(
        c => !(c.filterBy == source && c.filterValue == filterValue)
      );
    } else {
      this.filters.push(new Filter(checkbox, source, filterValue));
    }

    if (this.filters.length > 0) {
      this.rows = this.rowsCache
        .filter(r => this.isFilter(r, "country"))
        .filter(r => this.isFilter(r, "gender"))
        .filter(r => this.isFilter(r, "status"))
        .filter(r => this.isFilter(r, "spouseStatus"))
        .filter(r =>
            Utilities.searchArray(this.searchKeyword, false, [r.firstName_Ar,
                r.parent  == null ? null : r.parent.firstName_Ar,
            r.motherFirstName_Ar,
            r.motherLastName_Ar,
            r.spouseFirstName_Ar,
            r.spouseLastName_Ar,
            r.femChildrenList_En,
            r.firstName_En,
            r.familyName_En,
            r.parent  == null ? null : r.parent.firstName_En,
            r.motherFirstName_En,
            r.motherLastName_En,
            r.spouseFirstName_En,
            r.spouseLastName_En,
            r.femChildrenList_En
            ])
        );
    } else {
      this.rows = this.rowsCache.filter(r =>
          Utilities.searchArray(this.searchKeyword, false, [r.firstName_Ar,
              r.parent  == null ? null : r.parent.firstName_Ar,
          r.motherFirstName_Ar,
          r.motherLastName_Ar,
          r.spouseFirstName_Ar,
          r.spouseLastName_Ar,
          r.femChildrenList_En,
          r.firstName_En,
          r.familyName_En,
          r.parent  == null ? null : r.parent.firstName_En,
          r.motherFirstName_En,
          r.motherLastName_En,
          r.spouseFirstName_En,
          r.spouseLastName_En,
          r.femChildrenList_En
          ])
      );
    }

    this.refreshTree(this.rows);
  }

  isFilter(value: FamilyMember, filterBy: string): boolean {
    let sourceFilter = this.filters.filter(q => q.filterBy == filterBy);
    if (sourceFilter.length == 0) return true;

    if (filterBy == "country") {
      return (
        sourceFilter.filter(q => parseInt(q.filterValue) == value.countryId)
          .length > 0
      );
    }
    if (filterBy == "gender") {
      return (
        sourceFilter.filter(q => parseInt(q.filterValue) == value.gender)
          .length > 0
      );
    }
    if (filterBy == "status") {
      return (
        sourceFilter.filter(q => parseInt(q.filterValue) == (value.passedAway ? 1 : 0) )
          .length > 0
      );
    }
    if (filterBy == "spouseStatus") {
      return (
        sourceFilter.filter(q => (parseInt(q.filterValue) == (value.spousePassedAway ? 1 : 0)) && value.maritalStatus == MaritalStatus.Married )
          .length > 0
      );
    }
  }

  switchView() {
    this.isTree = !this.isTree;

    setTimeout(() => this.refreshTree(this.rows), 100);
  }

  toggleExpandRow(row) {
    console.log("Toggled Expand Row!", row);

    if (this.expandedRow && this.expandedRow.id != row.id) {
      this.table.rowDetail.collapseAllRows();
    }

    this.expandedRow = row;

    let rowHeight = 60;

    if (row.displayPhone) {
      rowHeight += 25;
    }

    if (row.displayEmail) {
      rowHeight += 25;
    }
    this.table.rowDetail.rowHeight = rowHeight;
    this.table.rowDetail.toggleExpandRow(row);
  }

  onDetailToggle(event) {
    console.log("Detail Toggled", event);
  }
}
