// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Gender, PhoneType, MaritalStatus } from "./enums";
import { selectRows } from "@swimlane/ngx-datatable/release/utils";

export class FamilyMember {
    // NOTE: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: number,
        familyId?: string,
        generation?: number,
        sequence?: number,
        firstName_Ar?: string,
        motherFirstName_Ar?: string,
        motherLastName_Ar?: string,
        spouseFirstName_Ar?: string,
        spouseLastName_Ar?: string,
        prvSpouseFirstName_Ar?: string,
        prvSpouseLastName_Ar?: string,
        prvPrvSpouseFirstName_Ar?: string,
        prvPrvSpouseLastName_Ar?: string,
        femChildrenList_Ar?: string,
        firstName_En?: string,
        familyName_En?: string,
        motherFirstName_En?: string,
        motherLastName_En?: string,
        spouseFirstName_En?: string,
        spouseLastName_En?: string,
        prvSpouseFirstName_En?: string,
        prvSpouseLastName_En?: string,
        prvPrvSpouseFirstName_En?: string,
        prvPrvSpouseLastName_En?: string,
        femChildrenList_En?: string,
        displayPhone?: string,
        displayEmail?: string,
        displayPhoneType?: PhoneType,
        countryId?: number,
        city?: string,
        province?: string,
        gender?: Gender,
        facebook?: string,
        linkedIn?: string,
        website?: string,
        deathYear?: number,
        birthYear?: number,
        maritalStatus?: MaritalStatus,
        horoscopeId?: number,
        passedAway?: boolean,
        spousePassedAway?: boolean,
        parentId?: number,
        parent?: FamilyMember) {

        this.id = id;
        this.familyId = familyId;
        this.firstName_Ar = firstName_Ar;
        this.motherFirstName_Ar = motherFirstName_Ar;
        this.motherLastName_Ar = motherLastName_Ar;
        this.spouseFirstName_Ar = spouseFirstName_Ar;
        this.spouseLastName_Ar = spouseLastName_Ar;
        this.prvSpouseFirstName_Ar = prvSpouseFirstName_Ar;
        this.prvSpouseLastName_Ar = prvSpouseLastName_Ar;
        this.prvPrvSpouseFirstName_Ar = prvPrvSpouseFirstName_Ar;
        this.prvPrvSpouseLastName_Ar = prvPrvSpouseLastName_Ar;
        this.femChildrenList_Ar = femChildrenList_Ar;

        this.firstName_En = firstName_En;
        this.familyName_En = familyName_En;
        this.motherFirstName_En = motherFirstName_En;
        this.motherLastName_En = motherLastName_En;
        this.spouseFirstName_En = spouseFirstName_En;
        this.spouseLastName_En = spouseLastName_En;
        this.prvSpouseFirstName_En = prvSpouseFirstName_En;
        this.prvSpouseLastName_En = prvSpouseLastName_En;
        this.prvPrvSpouseFirstName_En = prvPrvSpouseFirstName_En;
        this.prvPrvSpouseLastName_En = prvPrvSpouseLastName_En;
        this.femChildrenList_En = femChildrenList_En;
        this.city = city;
        this.province = province
        this.countryId = countryId;
        
        this.displayEmail = displayEmail;
        this.displayPhone = displayPhone;
        this.displayPhoneType = displayPhoneType;

        this.gender = gender;
        this.facebook = facebook;
        this.linkedIn = linkedIn;
        this.website = website;
        this.deathYear = deathYear;
        this.birthYear = birthYear;
        this.maritalStatus = maritalStatus;
        this.horoscopeId = horoscopeId;
        this.passedAway = passedAway;
        this.spousePassedAway = spousePassedAway;
        this.parentId = parentId;
        this.parent = parent;
        this.sequence = sequence;
        this.generation = generation;
    }

    public get fullName(): string {
        let name = this.firstName_Ar + " " + this.parent.firstName_Ar;

        return name;
    }


    public set fullName(value: string) {
        
    }


    public id: number;
    public familyId: string;
    public generation: number;
    public sequence: number;
    public firstName_Ar: string;
    public motherFirstName_Ar: string;
    public motherLastName_Ar: string;
    public spouseFirstName_Ar: string;
    public spouseLastName_Ar: string;
    public prvSpouseFirstName_Ar: string;
    public prvSpouseLastName_Ar: string;
    public prvPrvSpouseFirstName_Ar: string;
    public prvPrvSpouseLastName_Ar: string;
    public femChildrenList_Ar: string;
    public firstName_En: string;
    public familyName_En: string;
    public motherFirstName_En: string;
    public motherLastName_En: string;
    public spouseFirstName_En: string;
    public spouseLastName_En: string;
    public prvSpouseFirstName_En: string;
    public prvSpouseLastName_En: string;
    public prvPrvSpouseFirstName_En: string;
    public prvPrvSpouseLastName_En: string;
    public femChildrenList_En: string;
    public displayEmail: string;
    public displayPhone: string;
    public displayPhoneType: PhoneType;
    public city: string;
    public province: string;
    public countryId: number;
    public gender: Gender;
    public facebook?: string;
    public linkedIn?: string;
    public website?: string;
    public deathYear?: number;
    public birthYear?: number;
    public maritalStatus?: MaritalStatus;
    public horoscopeId?: number;
    public parentId: number;
    public parent: FamilyMember;
    public passedAway: boolean;
    public spousePassedAway: boolean;
    
    public get genderSelection(): boolean {
        return this.gender === Gender.Male;
    }

    public set genderSelection(value: boolean){
        value? this.gender = Gender.Male : this.gender = Gender.Female;
    }

    public get maritalStatusSelection(): boolean {
        return this.maritalStatus === MaritalStatus.Married;
    }

    public set maritalStatusSelection(value: boolean){
        value? this.maritalStatus = MaritalStatus.Married : this.maritalStatus = MaritalStatus.NotMarried;
    }
}