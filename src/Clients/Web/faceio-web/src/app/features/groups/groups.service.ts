import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ICreateGroupRequest, IGroupDto, IUpdateGroupRequest } from "./contracts/interfaces";
import { Observable } from "rxjs";
import { IPersonDto } from "../persons/contracts/interfaces";

@Injectable({
  providedIn: "root",
})
export class GroupsService {
  private baseUrl: string;

  constructor(private _httpClient: HttpClient) {
    this.baseUrl = "https://localhost:5001";
  }

  public getGroups(customerUid: string): Observable<IGroupDto[]>{
    return this._httpClient.get<IGroupDto[]>(`${this.baseUrl}/api/customers/${customerUid}/groups`);
  }

  public getPeopleInGroup(customerUid: string, groupUid: string): Observable<IPersonDto[]>{
    return this._httpClient.get<IPersonDto[]>(`${this.baseUrl}/api/customers/${customerUid}/groups/${groupUid}/people`);
  }

  public getGroup(customerUid: string, groupUid: string): Observable<IGroupDto>{
    return this._httpClient.get<IGroupDto>(`${this.baseUrl}/api/customers/${customerUid}/groups/${groupUid}`);
  }

  public removeGroup(customerUid: string, groupUid: string): Observable<void>{
    return this._httpClient.delete<void>(`${this.baseUrl}/api/customers/${customerUid}/groups/${groupUid}`);
  }

  public addGroup(customerUid: string, request: ICreateGroupRequest): Observable<void>{
    return this._httpClient.post<void>(`${this.baseUrl}/api/customers/${customerUid}/groups`, request);
  }

  public updateGroup(customerUid: string, locationUid: string, request: IUpdateGroupRequest): Observable<void>{
    return this._httpClient.patch<void>(`${this.baseUrl}/api/customers/${customerUid}/groups/${locationUid}`, request);
  }
}
