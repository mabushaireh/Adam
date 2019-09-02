// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { AppTranslationService } from "../../services/app-translation.service";
import { AccountService } from "../../services/account.service";
import { Utilities } from "../../services/utilities";
import { User } from '../../models/user.model';
import { Role } from '../../models/role.model';
import { Permission } from '../../models/permission.model';
import { UserEdit } from '../../models/user-edit.model';
import { UserInfoComponent } from "./user-info.component";


@Component({
    selector: 'users-management',
    templateUrl: './users-management.component.html',
    styleUrls: ['./users-management.component.css']
})
export class UsersManagementComponent implements OnInit, AfterViewInit {
    columns: any[] = [];
    rows: User[] = [];
    rowsCache: User[] = [];
    editedUser: UserEdit;
    sourceUser: UserEdit;
    editingUserName: { name: string };
    loadingIndicator: boolean;

    allRoles: Role[] = [];


    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('rolesTemplate')
    rolesTemplate: TemplateRef<any>;

    @ViewChild('fullNameTemplate')
    fullNameTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('editorModal')
    editorModal: ModalDirective;

    @ViewChild('userEditor')
    userEditor: UserInfoComponent;

    constructor(private alertService: AlertService, private translationService: AppTranslationService, private accountService: AccountService) {
    }


    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

        this.columns = [
            { prop: "index", name: '#', width: 20, cellTemplate: this.indexTemplate, canAutoResize: false },
            { prop: 'fullName', name: gT('usersControl.table.fullName'), width: 110, cellTemplate: this.fullNameTemplate },
            { prop: 'email', name: gT('usersControl.table.email'), width: 100 },
            { prop: 'roles', name: gT('usersControl.table.roles'), width: 80, cellTemplate: this.rolesTemplate },
            { prop: 'mobile', name: gT('usersControl.table.mobile'), width: 80 },
            //{ prop: 'isEnabled', name: gT('usersControl.table.status'), width: 100, cellTemplate: this.statusTemplate }

        ];

        if (this.canManageUsers)
            this.columns.push({ name: '', width: 250, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false });

        this.loadData();
    }





    ngAfterViewInit() {

        this.userEditor.changesSavedCallback = () => {
            this.addNewUserToList();
            this.editorModal.hide();
        };

        this.userEditor.changesCancelledCallback = () => {
            this.editedUser = null;
            this.sourceUser = null;
            this.editorModal.hide();
        };
    }


    addNewUserToList() {
        if (this.sourceUser) {
            Object.assign(this.sourceUser, this.editedUser);
            this.editedUser = null;
            this.sourceUser = null;
        }
        else {
            let user = new User();
            Object.assign(user, this.editedUser);
            this.editedUser = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>user).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, user);
            this.rows.splice(0, 0, user);
        }
    }




    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;

        if (this.canViewRoles) {
            this.accountService.getUsersAndRoles().subscribe(results => this.onDataLoadSuccessful(results[0], results[1]), error => this.onDataLoadFailed(error));
        }
        else {
            this.accountService.getUsers().subscribe(users => this.onDataLoadSuccessful(users, []), error => this.onDataLoadFailed(error));
        }
    }

    onDataLoadSuccessful(users: User[], roles: Role[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        users.forEach((user, index, users) => {
            (<any>user).index = index + 1;
            (<any>user).fullName = user.firstName + " " + user.lastName;
        });

        this.rowsCache = [...users];
        this.rows = users;

        this.allRoles = roles;
    }

    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.email, r.fullName, r.email, r.mobile, r.roles));
    }

    onEditorModalHidden() {
        this.editingUserName = null;
        this.userEditor.resetForm(true);
    }


    newUser() {
        this.editingUserName = null;
        this.sourceUser = null;
        this.editedUser = this.userEditor.newUser(this.allRoles);
        this.editorModal.show();
    }


    editUser(row: UserEdit) {
        this.editingUserName = { name: row.email };
        this.sourceUser = row;
        this.editedUser = this.userEditor.editUser(row, this.allRoles);
        this.editorModal.show();
    }

    deleteUser(row: UserEdit) {
        this.alertService.showDialog('Are you sure you want to delete \"' + row.email + '\"?', DialogType.confirm, () => this.deleteUserHelper(row));
    }


    deleteUserHelper(row: UserEdit) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.accountService.deleteUser(row)
            .subscribe(results => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;

                this.rowsCache = this.rowsCache.filter(item => item !== row)
                this.rows = this.rows.filter(item => item !== row)
            },
            error => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;

                this.alertService.showStickyMessage("Delete Error", `An error occured whilst deleting the user.\r\nError: "${Utilities.getHttpResponseMessage(error)}"`,
                    MessageSeverity.error, error);
            });
    }

    toggleStatus(row: UserEdit) {
        this.alertService.showDialog(this.translationService.getTranslation((row.isEnabled ? 'usersControl.table.messages.deactivateConfirmation' : 'usersControl.table.messages.activateConfirmation'), { email: row.email }), DialogType.confirm, () => this.toggleUserStatusHelper(row));
    }

    toggleUserStatusHelper(row: UserEdit) {

        this.alertService.startLoadingMessage(this.translationService.getTranslation(row.isEnabled ? 'usersControl.table.messages.deactivating' : 'usersControl.table.messages.activating'));
        this.loadingIndicator = true;

        this.accountService.changeUserStatus(row.id, !row.isEnabled)
            .subscribe(results => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;
                row.isEnabled = !row.isEnabled;
            },
            error => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;

                this.alertService.showStickyMessage(this.translationService.getTranslation('usersControl.table.messages.deactivateFailed.title'), this.translationService.getTranslation('usersControl.table.messages.deactivateFailed.message', { error: Utilities.getHttpResponseMessage(error) }),
                    MessageSeverity.error, error);
            });
    }




    get canViewRoles() {
        return this.accountService.userHasPermission(Permission.viewRolesPermission)
    }

    get canManageUsers() {
        return this.accountService.userHasPermission(Permission.manageUsersPermission);
    }

}
