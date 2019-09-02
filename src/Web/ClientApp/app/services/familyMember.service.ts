// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
//

// ======================================

import { Injectable } from "@angular/core";
import { Router, NavigationExtras } from "@angular/router";
import { Http, Headers, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import "rxjs/add/observable/forkJoin";
import "rxjs/add/operator/do";
import "rxjs/add/operator/map";

import { FamilyMemberEndpoint } from "./familyMember-endpoint.service";
import { AuthService } from "./auth.service";
import { FamilyMember } from "../models/familyMember.model";

@Injectable()
export class FamilyMemberService {
  constructor(
    private router: Router,
    private http: Http,
    private authService: AuthService,
    private familyMemberEndpoint: FamilyMemberEndpoint
  ) {}

  getFamilyMembers(page?: number, pageSize?: number) {
    return this.familyMemberEndpoint
      .getFamilyMembersEndpoint(page, pageSize)
      .map((response: Response) => <FamilyMember[]>response.json());
  }

  refreshLoggedInUser() {
    return this.authService.refreshLogin();
  }

  requestUpdateFamilyMember(familyMember: FamilyMember) {
    return this.familyMemberEndpoint.getRequestUpdateFamilyMemberEndpoint(
      familyMember,
      familyMember.id
    );
  }
}
