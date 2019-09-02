// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Injectable } from '@angular/core';

import { AppTranslationService } from './app-translation.service';
import { LocalStoreManager } from './local-store-manager.service';
import { DBkeys } from './db-Keys';
import { Utilities } from './utilities';



type UserConfiguration = {
    language: string
};

@Injectable()
export class ConfigurationService {

    public static readonly appVersion: string = "1.0.0";

    public baseUrl: string = Utilities.baseUrl().replace(/\/$/, '');
    public fallbackBaseUrl: string = "";
    public loginUrl: string = "/Login";

    //***Specify default configurations here***
    // TODO: enable Arabic Language
    public static readonly defaultLanguage: string = "en";

    //***End of defaults***  

    private _language: string = null;
  

    constructor(private localStorage: LocalStoreManager, private translationService: AppTranslationService) {
        this.loadLocalChanges();
    }



    private loadLocalChanges() {

        if (this.localStorage.exists(DBkeys.LANGUAGE)) {
            this._language = this.localStorage.getDataObject<string>(DBkeys.LANGUAGE);
            this.translationService.changeLanguage(this._language);
        }
        else {
            this.resetLanguage();
        }
    }


    private saveToLocalStore(data: any, key: string) {
        setTimeout(() => this.localStorage.savePermanentData(data, key));
    }


    public import(jsonValue: string) {

        this.clearLocalChanges();

        if (!jsonValue)
            return;

        let importValue: UserConfiguration = Utilities.JSonTryParse(jsonValue);

        if (importValue.language != null)
            this.language = importValue.language;
    }


    public export(changesOnly = true): string {

        let exportValue: UserConfiguration =
            {
                language: changesOnly ? this._language : this.language,
            };

        return JSON.stringify(exportValue);
    }


    public clearLocalChanges() {
        this._language = null;

        this.localStorage.deleteData(DBkeys.LANGUAGE);

        this.resetLanguage();
    }


    private resetLanguage() {
        let language = ConfigurationService.defaultLanguage;

        if (language) {
            this._language = language;
        }
        else {
            this._language = this.translationService.changeLanguage(language);
        }
    }

    
    set language(value: string) {
        // console.debug ("Configuration Language Set to : " + value);
        this._language = value;
        this.saveToLocalStore(value, DBkeys.LANGUAGE);
        this.translationService.changeLanguage(value);
    }
    get language() {
        if (this._language != null)
        {
            // console.debug ("Configuration Language is: " + this._language);
            return this._language;
        }

        // console.debug ("Configuration Language is  Default: " + ConfigurationService.defaultLanguage);

        return ConfigurationService.defaultLanguage;
    }
}