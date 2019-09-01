// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

export class User {
    // NOTE: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, email?: string, firstName?: string, lastName?: string, mobile?: string, roles?: string[]) {

        this.id = id;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.mobile = mobile;
        this.roles = roles;
    }


    public get friendlyName(): string {
        return this.fullName;
    }

    public get fullName(): string {
        let name = this.firstName + " " + this.lastName;

        return name;
    }

    public id: string;
    public firstName: string;
    public lastName: string;
    public email: string;
    public mobile: string;
    public isEnabled: boolean;
    public isLockedOut: boolean;
    public roles: string[];
}