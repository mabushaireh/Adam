// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router, NavigationStart } from '@angular/router';
import { Location } from '@angular/common';


import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AuthService } from "../../services/auth.service";
import { ConfigurationService } from '../../services/configuration.service';
import { Utilities } from '../../services/utilities';
import { AppTranslationService } from '../../services/app-translation.service';
import { User } from '../../models/user.model';
import { UserEdit } from '../../models/user-edit.model';

import { fadeInOut } from '../../services/animations';
import { Permission } from '../../models/permission.model';


@Component({
    selector: 'signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css'],
    animations: [fadeInOut]
})
export class SignUpComponent implements OnInit {
    isLoading = false;
    formResetToggle = true;
    signUpStatusSubscription: any;
    private showValidationErrors = false;
    newUser: UserEdit = new UserEdit();
    public uniqueId: string = Utilities.uniqueId();


    @ViewChild('f')
    private form;

    constructor(private alertService: AlertService,
        private router: Router,
        private authService: AuthService,
        private tranlsationService: AppTranslationService,
        private _location: Location) {
        this.showValidationErrors = true;
    }

    ngOnInit(): void {

    }

    resetForm(replace = false) {
        if (!replace) {
            this.form.reset();
        }
        else {
            this.formResetToggle = false;

            setTimeout(() => {
                this.formResetToggle = true;
            });
        }
    }

    cancel() {
        this._location.back();
    }

    save() {
        this.isLoading = true;

        this.alertService.startLoadingMessage(this.tranlsationService.getTranslation('signUp.signingUp'));

        this.authService.signUp(this.newUser).subscribe(user => this.signUpSuccessHelper(user), error => this.saveFailedHelper(error));
    }


    private signUpSuccessHelper(user?: User) {

        this.isLoading = false;

        this.alertService.stopLoadingMessage();

        this.newUser = new UserEdit();

        this.resetForm();

        this.alertService.showMessage(this.tranlsationService.getTranslation('signUp.messages.success.title'), this.tranlsationService.getTranslation('signUp.messages.success.message', { email: user.email }), MessageSeverity.success);
        this._location.back();
    }


    private saveFailedHelper(error: any) {
        this.isLoading = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(this.tranlsationService.getTranslation('signUp.messages.failed.title'), this.tranlsationService.getTranslation('signUp.messages.failed.message', { errorMsg: error }), MessageSeverity.error, error);
        this.alertService.showStickyMessage(error, null, MessageSeverity.error);

    }

    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }

    get firstName_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.firstName.validation.required.title');
    }
    get firstName_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.firstName.validation.required.message');
    }

    get lastName_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.lastName.validation.required.title');
    }
    get lastName_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.lastName.validation.required.message');
    }
    get mobile_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.mobile.validation.required.title');
    }
    get mobile_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.mobile.validation.required.message');
    }
    get mobile_validation_pattern_title(): string {
        return this.tranlsationService.getTranslation('signUp.mobile.validation.pattern.title');
    }
    get mobile_validation_pattern_message(): string {
        return this.tranlsationService.getTranslation('signUp.mobile.validation.pattern.message');
    }
    get email_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.email.validation.required.title');
    }
    get email_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.email.validation.required.message');
    }
    get email_validation_pattern_title(): string {
        return this.tranlsationService.getTranslation('signUp.email.validation.pattern.title');
    }
    get email_validation_pattern_message(): string {
        return this.tranlsationService.getTranslation('signUp.email.validation.pattern.message');
    }
    get password_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.password.validation.required.title');
    }
    get password_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.password.validation.required.message');
    }
    get password_validation_pattern_title(): string {
        return this.tranlsationService.getTranslation('signUp.password.validation.pattern.title');
    }
    get password_validation_pattern_message(): string {
        return this.tranlsationService.getTranslation('signUp.password.validation.pattern.message');
    }
    get confirmPassword_validation_required_title(): string {
        return this.tranlsationService.getTranslation('signUp.confirmPassword.validation.required.title');
    }
    get confirmPassword_validation_required_message(): string {
        return this.tranlsationService.getTranslation('signUp.confirmPassword.validation.required.message');
    }
    get confirmPassword_validation_mismatch_title(): string {
        return this.tranlsationService.getTranslation('signUp.confirmPassword.validation.mismatch.title');
    }
    get confirmPassword_validation_mismatch_message(): string {
        return this.tranlsationService.getTranslation('signUp.confirmPassword.validation.mismatch.message');
    }
}
