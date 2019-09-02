// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

export type PermissionNames =
    "View Members" | "Manage Members" |
    "View Roles" | "Manage Roles" | "Assign Roles" | "View Templates" | "Request Templates" | "View Request" | "Approve Requests";

export type PermissionValues =
    "members.view" | "members.manage" |
    "roles.view" | "roles.manage" | "roles.assign" | "templates.view" | "templates.request" | "requests.view" | "requests.approve";

export class Permission {

    public static readonly viewUsersPermission: PermissionValues = "members.view";
    public static readonly manageUsersPermission: PermissionValues = "members.manage";

    public static readonly viewRolesPermission: PermissionValues = "roles.view";
    public static readonly manageRolesPermission: PermissionValues = "roles.manage";
    public static readonly assignRolesPermission: PermissionValues = "roles.assign";

    constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
        this.name = name;
        this.value = value;
        this.groupName = groupName;
        this.description = description;
    }

    public name: PermissionNames;
    public value: PermissionValues;
    public groupName: string;
    public description: string;
}