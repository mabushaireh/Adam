// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { Http, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/observable/forkJoin';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { LookupItemEndpoint } from './lookupItem-endpoint.service';
import { AuthService } from './auth.service';
import { LookupItem } from '../models/lookupItem.model';
import { LocaleCategory } from "../models/enums";



@Injectable()
export class LookupItemService {
    
    constructor(private router: Router, private http: Http, private authService: AuthService,
        private lookupItemEndpoint: LookupItemEndpoint) {
    }
    
    getLookupItemsByCategory(localeCategory: LocaleCategory) {
        return this.lookupItemEndpoint.getLookupItemsByCategoryEndpoint(localeCategory)
            .map((response: Response) => <LookupItem[]>response.json());
    }

    refreshLoggedInUser() {
        return this.authService.refreshLogin();
    }
}