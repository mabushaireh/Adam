// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

import { Injectable, Injector } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';
import { LocaleCategory } from "../models/enums";


@Injectable()
export class LookupItemEndpoint extends EndpointFactory {

    private readonly _lookupItemUrl: string = "/api/lookupItem";
    

    get lookupItemUrl() { return this.configurations.baseUrl + this._lookupItemUrl; }
    
    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }


    getLookupItemsByCategoryEndpoint(localeCategory: LocaleCategory): Observable<Response> {
        let endpointUrl = `${this.lookupItemUrl}/params/${localeCategory}`;

        return this.http.get(endpointUrl, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getLookupItemsByCategoryEndpoint(localeCategory));
            });
    }
}