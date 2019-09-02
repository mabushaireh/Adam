// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Injectable, Injector } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class FamilyMemberEndpoint extends EndpointFactory {

    private readonly _familyMembersUrl: string = "/api/familyMember";
    

    get familyMembersUrl() { return this.configurations.baseUrl + this._familyMembersUrl; }
    
    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }


    getFamilyMembersEndpoint(page?: number, pageSize?: number): Observable<Response> {
        let endpointUrl = page && pageSize ? `${this.familyMembersUrl}/${page}/${pageSize}` : this.familyMembersUrl;

        return this.http.get(endpointUrl, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getFamilyMembersEndpoint(page, pageSize));
            });
    }

    getRequestUpdateFamilyMemberEndpoint(familyMemberObj: any, familyMemberId?: number): Observable<Response> {
        let endpointUrl = `${this.familyMembersUrl}/${familyMemberId}`;

        return this.http.put(endpointUrl, JSON.stringify(familyMemberObj), this.getAuthHeader(true))
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getRequestUpdateFamilyMemberEndpoint(familyMemberObj, familyMemberId));
            });
    }
}